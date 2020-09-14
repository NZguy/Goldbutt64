using Assets.Scripts.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AttributeBuilder : MonoBehaviour
{

    public Player player;
    public TMP_Dropdown AttType;
    public TMP_InputField Name;
    public TMP_InputField Flat;
    public TMP_InputField Percent;

    public void CreateAttribute ()
    {
        AttributeType type = AttributeType.Snakes;
        Enum.TryParse(AttType.options[AttType.value].text, out type);


        string name = string.IsNullOrEmpty(Name.text) ? "Missing Name" : Name.text;
        float value = 0;
        float flat = float.TryParse(Flat.text, out value) ? value : 0;
        float percent = float.TryParse(Percent.text, out value) ? value : 0;

        player.AddAttribute(new AttributeEntity(type) {
            Name = name,
            FlatValue = flat,
            PercentValue = percent
        });
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
