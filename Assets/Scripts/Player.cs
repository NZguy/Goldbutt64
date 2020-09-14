using Assets.Scripts.Attributes;
using Assets.Scripts.Events.Base;
using Assets.Scripts.Events.OnEvents;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NotifierBase, ISubscriber
{
    public GameObject primaryWeapon;
    public AttributeManager Attributes;
    private Rigidbody rb;
    private bool IsMoving;

    public float Health;

    void Start()
    {
        this.rb = GetComponent<Rigidbody>();

        // Some default attributes for testing / playing with. 
        #region Test Attributes
        AddAttribute(new AttributeEntity(AttributeType.AttackSpeed)
        {
            FlatValue = 5,
            PercentValue = 60
        });

        AddAttribute(new AttributeEntity(AttributeType.Lives)
        {
            FlatValue = 1,
            PercentValue = 0
        });

        AddAttribute(new AttributeEntity(AttributeType.MovementSpeed)
        {
            FlatValue = 5,
        });

        AddAttribute(new AttributeEntity(AttributeType.MovementSpeed)
        {
            FlatValue = 5,
            PercentValue = 50.0f
        });

        AddAttribute(new AttributeEntity(AttributeType.MovementSpeed)
        {
            FlatValue = 5,
            PercentValue = 0
        });

        AddAttribute(new AttributeEntity(AttributeType.MovementSpeed)
        {
            FlatValue = 5,
            PercentValue = -90.0f
        });

        AddAttribute(new AttributeEntity(AttributeType.Health)
        {
            FlatValue = 10,
            PercentValue = .5f
        });

        AddAttribute(new AttributeEntity(AttributeType.HealthRegen)
        {
            FlatValue = 10,
            PercentValue = .5f
        });

        AddAttribute(new AttributeEntity(AttributeType.Mana)
        {
            FlatValue = 10,
            PercentValue = .5f
        });
        AddAttribute(new AttributeEntity(AttributeType.ManaRegen)
        {
            FlatValue = 10,
            PercentValue = .5f
        });
        AddAttribute(new AttributeEntity(AttributeType.Piercing)
        {
            FlatValue = 10,
            PercentValue = .5f
        });
        AddAttribute(new AttributeEntity(AttributeType.PiercingResistance)
        {
            FlatValue = 10,
            PercentValue = .5f
        });
        #endregion
    }

    void Update()
    {
        transform.rotation = GetCharacterRotation();
        rb.velocity = new Vector3(Input.GetAxis("Horizontal_Move") * Attributes.GetValue(AttributeType.MovementSpeed), 0, Input.GetAxis("Vertical_Move") * Attributes.GetValue(AttributeType.MovementSpeed));

        bool IsMovingNew = rb.velocity.magnitude > 0;
        if (IsMovingNew && !IsMoving)
        {   // Just starting moving - better let my many subscribers know!
            Notify(new OnStartMoving(this, this));

        } else if (!IsMovingNew && IsMoving)
        {   // Just stopped moving - better let my many subscribers know!
            Notify(new OnStopMoving(this, this));
        }

        

        IsMoving = IsMovingNew;
        //Debug.Log($"Number of Attributes: {Attributes.FinalValues.Count}");
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

    public void AddAttribute(AttributeEntity att)
    {
        Attributes.Add(att);
        // These should will be moved out of the update loop or deleted entirely when we have functionality for picking up items / applying attributes / etc.
        Notify(new OnTextUpdate(this, "StatsAttributes", Attributes.ToStringTableCompleteRich()));
        Notify(new OnTextUpdate(this, "StatsFinalValues", Attributes.ToStringTableFinalValues()));
        Notify(new OnAttributeAdd(this, att));
    }

    public void RemoveAttribute(AttributeEntity att)
    {
        Attributes.Remove(att);
        Notify(new OnTextUpdate(this, "StatsAttributes", Attributes.ToStringTableCompleteRich()));
        Notify(new OnTextUpdate(this, "StatsFinalValues", Attributes.ToStringTableFinalValues()));
        Notify(new OnAttributeRemove(this, att));
    }
    
    public bool OnNotify(IGameEvent gameEvent)
    {
        throw new System.NotImplementedException();
    }
}
