using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Vector3 RotationOffset;
    public string nameOfGameObjectToLookAt;
    public GameObject gameObjectToLookAt;
    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(nameOfGameObjectToLookAt))    
            gameObjectToLookAt = GameObject.Find(nameOfGameObjectToLookAt);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(gameObjectToLookAt.transform);
        this.transform.Rotate(RotationOffset);
    }
}
