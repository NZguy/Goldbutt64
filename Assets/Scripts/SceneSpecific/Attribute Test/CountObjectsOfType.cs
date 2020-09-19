using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountObjectsOfType : MonoBehaviour
{

    public TextMeshProUGUI Text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text.text = $"Projectiles: {GameObject.FindGameObjectsWithTag("Projectile").Length}";
        
    }
}
