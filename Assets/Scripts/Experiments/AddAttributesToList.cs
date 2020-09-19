using Assets.Scripts.Attributes;
using Assets.Scripts.Events.OnEvents;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AddAttributesToList : MonoBehaviour, ISubscriber
{
    private Player player;
    public ScrollRect ScrollView;
    public GameObject ScrollContent;
    public GameObject prefab;

    public bool OnNotify(IGameEvent gameEvent)
    {
        if (gameEvent is OnAttributeAdd attAddEvent)
        {

            /////////////
            /// This needs to be replaced by a parent class that overrides whatever unload/dispose method unity uses to also unsubscribe events.
            if (ScrollView == null)
            {
                //Debug.Log("Scrollview was null");
                return true;
            }

            if (ScrollContent == null)
            {
                //Debug.Log("ScrollContent was null");
                return true;
            }
            /////////////
            var newAttribute = Instantiate(prefab);
            //newAttribute.GetComponent<AttributeRemove>().parent = (Player)attAddEvent.EventCreater;
            newAttribute.GetComponent<AttributeRemove>().attribute = attAddEvent.Attribute;
            newAttribute.transform.SetParent(ScrollContent.transform, false);
            newAttribute.SetActive(true);


            var title = newAttribute.transform.Find("Panel").transform.Find("Title").GetComponent<TextMeshProUGUI>();
            var details = newAttribute.transform.Find("Panel").transform.Find("Details").GetComponent<TextMeshProUGUI>();
            title.text = attAddEvent.Attribute.Name;
            details.text = $"Flat:{attAddEvent.Attribute.FlatValue} Percent:{attAddEvent.Attribute.PercentValue}";
        }
        return false;
    }

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.Subscribe(new OnAttributeAdd(null, null), this);
        
        foreach (AttributeEntity att in player.GetAttributes())
        {
            OnNotify(new OnAttributeAdd(player, att));
        }

    }
}
