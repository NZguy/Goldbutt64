using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    public ActionBase PrimaryAction;
    public ActionBase SecondaryAction;

    private Rigidbody rb;
    public float speed = 10;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.rotation = GetCharacterRotation();
        rb.velocity = new Vector3(Input.GetAxis("Horizontal_Move") * speed, 0, Input.GetAxis("Vertical_Move") * speed);
        UpdateAction(PrimaryAction, "Fire1");
        UpdateAction(SecondaryAction, "Fire2"); 
    }

    private void UpdateAction(IAction action, string key)
    {
        if (action != null)
        {
            action.Update();
            if (Input.GetButtonDown(key))
                action.Act();
        }
    } 

    private Quaternion GetCharacterRotation()
    {
        // Will need to be abstracted away to support different peripherals (console controllers / mouse / etc)
        // Should probably not be instantiating variables in the update loop like this
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(-90, -angle + 180, 0);
    }
}
