using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSceneLoader : MonoBehaviour
{
    private void Awake()
    {
        SceneController.Instance.LoadInitialScene();
    }
}
