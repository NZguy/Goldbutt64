using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    public string destinationSceneName;
    public string myTag;
    public string destinationTag;
    private GameObject transitioningGameObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transitioningGameObject = other.gameObject;
            SceneController.TransitionToScene(transitioningGameObject, destinationSceneName, destinationTag);
        }
    }

   
}
