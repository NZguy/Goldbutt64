using Assets.Scripts.Attributes;
using Assets.Scripts.Mods;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;


public class GunModContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject ModInfoTextGameObject;
    private TextMeshPro _textObject;

    [SerializeField]
    public List<Mod> AttachedMods;

    // Start is called before the first frame update
    void Start()
    {
        if (ModInfoTextGameObject != null)
            _textObject = ModInfoTextGameObject.GetComponent<TextMeshPro>();
        AttachedMods = new List<Mod>();
        ToggleText(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        ToggleText(true);
        if (AttachedMods == null || AttachedMods.Count <= 0)
        {
            SetText("No Attached Mods");
        }
        else
        {
            string modDescriptions = "";
            foreach (Mod mod in AttachedMods)
            {
                modDescriptions += mod.GetDescription();
                modDescriptions += "\n";
            }
            SetText(modDescriptions);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Gun gun = other.GetComponent<Player>().gun;
            gun.AttachedMods = AttachedMods;
        }
    }

    private void OnMouseUpAsButton()
    {
        this.enabled = false;
    }

    private void OnMouseExit()
    {
        ToggleText(false);
    }

    private void ToggleText(bool enable)
    {
        if (_textObject != null)
        {
            _textObject.enabled = enable;
        }
    }

    private void SetText(string newText)
    {
        if (_textObject != null && newText != null)
        {
            _textObject.text = newText;
        }
    }






    
}
