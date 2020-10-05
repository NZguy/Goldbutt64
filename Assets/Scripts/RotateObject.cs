using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    float SpeedX = 0;

    [SerializeField]
    float SpeedY = 100;

    [SerializeField]
    float SpeedZ = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(SpeedX, SpeedY, SpeedZ) * Time.deltaTime);
    }
}
