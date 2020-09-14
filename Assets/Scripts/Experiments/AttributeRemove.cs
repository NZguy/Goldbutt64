using Assets.Scripts.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttributeRemove : MonoBehaviour
{

    public Player parent;
    public AttributeEntity attribute;
    public GameObject parentPanel;
    public UnityEngine.UI.Button button;

    void Start()
    {
        parent = GameObject.Find("Player").GetComponent<Player>();
    }

    public void RemoveAttribute()
    {
        parent.RemoveAttribute(attribute);
        Destroy(parentPanel);
    }
}
