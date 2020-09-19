using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddAttributesTypesToDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (AttributeType t in Enum.GetValues(typeof(AttributeType)))
        {
            options.Add(new TMP_Dropdown.OptionData(t.ToString()));
        }
        dropdown.AddOptions(options);
        Destroy(this);
    }
}
