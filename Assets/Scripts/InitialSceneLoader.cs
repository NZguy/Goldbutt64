using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSceneLoader : MonoBehaviour
{
    public string defaultSceneName = "SampleScene";

    private void Awake()
    {
        SceneController.Instance.LoadDefaultScene(defaultSceneName);
    }
}
