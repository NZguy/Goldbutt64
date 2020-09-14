using Assets.Scripts.Events.Base;
using Assets.Scripts.Events.OnEvents;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBinder : MonoBehaviour, ISubscriber
{
    public TextMeshProUGUI Text;
    private NotifierBase Parent;
    public string[] startTags;
    public string[] endTags;

    // When notifier fires off an OnTextUpdate it will provide a string representing the variable that changed.
    // VariableToListenFor should match OnTextUpdate.VariableName.
    public string VariableToListenFor;

    public bool OnNotify(IGameEvent gameEvent)
    {
        if (gameEvent is OnTextUpdate newText) {
            if (VariableToListenFor.Equals(newText.VariableName))
                Text.text = AddTagsToString(newText.NewValue);
        }
        return false;
    }

    /// <summary>
    /// Adds configured tags from startTags and endTags to the beginning and ending of s respectively.
    /// </summary>
    /// <param name="s"> The string to be enclosed by startTags/endTags</param>
    /// <returns></returns>
    private string AddTagsToString(string s)
    {
        if (startTags == null || startTags.Length <= 0)
            return s;

        StringBuilder sb = new StringBuilder();
        foreach(string tag in startTags)
        {
            sb.Append(tag);
        }
        sb.Append(s);
        foreach (string tag in endTags)
        {
            sb.Append(tag);
        }
        return sb.ToString();
    }

    void Start()
    {
        Parent = GameObject.Find("Player").GetComponent<Player>();
        Parent.Subscribe(new OnTextUpdate(null, null, null), this);
        Array.Reverse(endTags);
    }

    void Update()
    {
    }
}
