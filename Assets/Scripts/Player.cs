using Assets.Scripts;
using Assets.Scripts.Attributes;
using Assets.Scripts.Events.Base;
using Assets.Scripts.Events.OnEvents;
using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public GameObject primaryWeapon;
    private Rigidbody rb;
    private bool IsMoving;
    public Gun gun;
    public float Health;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        #region Test Attributes

        AddAttribute(new AttributeEntity(AttributeType.MovementSpeed, 30, 1));
        AddAttribute(new AttributeEntity(AttributeType.RateOfFire, 1, 0));
        AddAttribute(new AttributeEntity(AttributeType.Size, 1, 0));


        AddAttribute(new AttributeEntity(AttributeType.ProjectileMass, 0.0001f, 0));
        AddAttribute(new AttributeEntity(AttributeType.ProjectileDamage, 10, 0));
        AddAttribute(new AttributeEntity(AttributeType.ProjectileBounce, 5, 0));
        AddAttribute(new AttributeEntity(AttributeType.ProjectileSpeed, 15, 0));
        AddAttribute(new AttributeEntity(AttributeType.RateOfFire, 10, 0));

        #endregion
        EquipItem(primaryWeapon.GetComponent<Gun>());
        UpdateAttributes();
    }

    void Update()
    {
        UpdateAttributes();
        transform.rotation = GetCharacterRotation();
        rb.velocity = new Vector3(Input.GetAxis("Horizontal_Move") * GetAttributeValue(AttributeType.MovementSpeed), 0, Input.GetAxis("Vertical_Move") * GetAttributeValue(AttributeType.MovementSpeed));
        this.transform.localScale = new Vector3(GetAttributeValue(AttributeType.Size), GetAttributeValue(AttributeType.Size), GetAttributeValue(AttributeType.Size));

        HandleAttack();
    }

    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && GUIUtility.hotControl == 0)
            primaryWeapon.GetComponent<Gun>().Attack(true);
        else if (Input.GetButton("Fire1") && GUIUtility.hotControl == 0)
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
        return Quaternion.Euler(0, -angle + 180, 0);
    }

    // This probably shouldn't be "Gun" but a base item class instead
    public void EquipItem(Gun newItem)
    {
        gun = newItem;
        gun.Init(this);
    }
    public void UnEquipItem()
    {
        gun.RevokeAttributes(this);
    } 
}
