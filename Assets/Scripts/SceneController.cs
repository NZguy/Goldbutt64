using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used to transition between scenes.
/// </summary>
public class SceneController : MonoBehaviour
{

    protected static SceneController instance;
    private Scene currentScene;
    private string destinationSceneName;
    private string destinationTag;
    private GameObject transitioningGameObject;
    private string defaultSceneName;
    private bool isTransitioningLevel;

    public static SceneController Instance 
    {
        get {
            if (instance != null)
                return instance;

            instance = FindObjectOfType<SceneController>();

            if (instance != null)
                return instance;

            Create();

            return instance;
        }
    }

    public static SceneController Create()
    {
        GameObject sceneControllerGameObject = new GameObject("SceneController");
        instance = sceneControllerGameObject.AddComponent<SceneController>();

        return instance;
    }

    void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);

        this.isTransitioningLevel = false;
    }

    public void LoadDefaultScene(string defaultSceneName)
    {
        this.defaultSceneName = defaultSceneName;
        SceneManager.sceneLoaded += initialSceneLoaded;
        SceneManager.LoadScene(defaultSceneName);
        SceneManager.LoadScene("NeverUnload", LoadSceneMode.Additive);
    }

    void initialSceneLoaded(Scene newScene, LoadSceneMode loadMode)
    {
        Debug.Log("Done Loading " + newScene.name);
        if (newScene.name == defaultSceneName)
        {
            SceneManager.sceneLoaded -= initialSceneLoaded;
            this.currentScene = newScene;
        }
    }

    public static void TransitionToScene(GameObject transitioningGameObject, string destinationSceneName, string destinationTag)
    {
        if(Instance.isTransitioningLevel == false)
        {
            Instance.isTransitioningLevel = true;
            Instance.StartCoroutine(Instance.Transtion(transitioningGameObject, destinationSceneName, destinationTag));
        }
    }

    private IEnumerator Transtion(GameObject transitioningGameObject, string destinationSceneName, string destinationTag)
    {
        this.transitioningGameObject = transitioningGameObject;
        this.destinationSceneName = destinationSceneName;
        this.destinationTag = destinationTag;

        SceneManager.LoadSceneAsync(destinationSceneName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += SceneLoaded;

        yield return null;
    }

    private Door GetDestination(string destinationTag)
    {
        Door[] entrances = FindObjectsOfType<Door>();
        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].myTag == destinationTag)
                return entrances[i];
        }
        Debug.LogWarning("No entrance was found with the " + destinationTag + " tag.");
        return null;
    }

    private void SetEnteringGameObjectLocation(Door entrance, GameObject transitioningGameObject)
    {
        if (entrance == null)
        {
            Debug.LogWarning("Entering Transform's location has not been set.");
            return;
        }
        Transform entranceLocation = entrance.transform.parent;
        Transform enteringTransform = transitioningGameObject.transform;
        enteringTransform.position = entranceLocation.position - (entranceLocation.transform.right * 4) + new Vector3(0,0,0);
        enteringTransform.rotation = entranceLocation.rotation;
    }

    void SceneLoaded(Scene newScene, LoadSceneMode loadMode)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        
        Debug.Log("Done Loading " + newScene.name);
        Scene destinationScene = SceneManager.GetSceneByName(destinationSceneName);
        if (destinationScene.IsValid())
        {
            Debug.Log("Scene is Valid");
            SceneManager.SetActiveScene(destinationScene);
            Door entrance = GetDestination(destinationTag);
            SetEnteringGameObjectLocation(entrance, this.transitioningGameObject);
            GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>().cameraSnapFlag = true;
            SceneManager.sceneUnloaded += SceneUnloaded;
            SceneManager.UnloadSceneAsync(currentScene);
        }
        Instance.isTransitioningLevel = false;
        Debug.Log("Scene Activated!");
    }

    void SceneUnloaded(Scene oldScene)
    {
        this.currentScene = GetDestination(destinationTag).gameObject.scene;
    }
}
