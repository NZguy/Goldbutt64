using Assets.Scripts.Events.Base;
using Assets.Scripts.Events.OnEvents;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NotifierBase, ISubscriber
{

    public GameObject primaryWeapon;

    private Rigidbody rb;
    public float speed = 10;
    private bool IsMoving;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.rotation = GetCharacterRotation();
        rb.velocity = new Vector3(Input.GetAxis("Horizontal_Move") * speed, 0, Input.GetAxis("Vertical_Move") * speed);

        bool IsMovingNew = rb.velocity.magnitude > 0;
        if (IsMovingNew && !IsMoving)
        {   // Just starting moving - better let my many subscribers know!
            Notify(new OnStartMoving(this, this));

        } else if (!IsMovingNew && IsMoving)
        {   // Just stopped moving - better let my many subscribers know!
            Notify(new OnStopMoving(this, this));
        }

        IsMoving = IsMovingNew;

        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
            primaryWeapon.GetComponent<Gun>().Attack(true);
        else if (Input.GetButton("Fire1"))
            primaryWeapon.GetComponent<Gun>().Attack(false);
    } 

    private Quaternion GetCharacterRotation()
    {
        var mousePos = Input.mousePosition;
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        float distance = 0;
        Vector3 targetPos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (playerPlane.Raycast(ray, out distance))
        {
            targetPos = ray.GetPoint(distance);
        }
        var relativePos = targetPos - transform.position;
        var angle = Mathf.Atan2(relativePos.z, relativePos.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(-90, -angle + 180, 0);
    }

    
    public bool OnNotify(IGameEvent gameEvent)
    {
        throw new System.NotImplementedException();
    }
}
