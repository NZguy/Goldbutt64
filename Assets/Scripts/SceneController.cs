using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
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

        DontDestroyOnLoad(gameObject);
    }

    public static void TransitionToScene(GameObject transitioningGameObject, string destinationSceneName, string destinationTag)
    {
        Instance.StartCoroutine(Instance.Transtion(transitioningGameObject, destinationSceneName, destinationTag));
    }

    private IEnumerator Transtion(GameObject transitioningGameObject, string destinationSceneName, string destinationTag)
    {
        DontDestroyOnLoad(transitioningGameObject);
        this.transitioningGameObject = transitioningGameObject;
        this.destinationSceneName = destinationSceneName;
        this.destinationTag = destinationTag;
        this.currentScene = SceneManager.GetActiveScene();
        print(currentScene.name);

        AsyncOperation scene = SceneManager.LoadSceneAsync(destinationSceneName, LoadSceneMode.Additive);
        scene.allowSceneActivation = false;
        SceneManager.sceneLoaded += SceneLoaded;

        while (scene.progress < 0.9f)
        {
            Debug.Log("Loading scene " + " [][] Progress: " + scene.progress);
            yield return null;
        }

        scene.allowSceneActivation = true;

        while (!scene.isDone)
        {
            // wait until it is really finished
            yield return null;
        }
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
        enteringTransform.position = entranceLocation.position - (entranceLocation.transform.right * 4);
        enteringTransform.rotation = entranceLocation.rotation;
    }

    void SceneLoaded(Scene newScene, LoadSceneMode loadMode)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        
        Debug.Log("Done Loading Scene");
        Scene destinationScene = SceneManager.GetSceneByName(destinationSceneName);
        if (destinationScene.IsValid())
        {
            Debug.Log("Scene is Valid");
            SceneManager.MoveGameObjectToScene(this.transitioningGameObject, newScene);
            SceneManager.SetActiveScene(destinationScene);
            Door entrance = GetDestination(destinationTag);
            SetEnteringGameObjectLocation(entrance, this.transitioningGameObject);
            GameObject.FindWithTag("MainCamera").GetComponent<CameraFollow>().cameraSnapFlag = true;
            SceneManager.UnloadSceneAsync(currentScene);
        }
        Debug.Log("Scene Activated!");
    }
}
