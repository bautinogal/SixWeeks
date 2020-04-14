using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manage weapons actions, selection, etc.
public class WeaponController : MonoBehaviour
{
    Weapon[] weapons;
    Weapon activeWeapon;

    public enum BulletMechanic { OnTrigger, Raycast}
    public BulletMechanic bulletMechanic = BulletMechanic.OnTrigger;

    //Looks for all the weapons in childs
    private void Start()
    {
        weapons = GetComponentsInChildren<Weapon>();
        if (weapons != null && weapons.Length > 0)
        {
            SwitchWeapon(0);
            Debug.Log("Weapon Controller: " + weapons.Length.ToString() +" weapons found!");
        }
        else
        {
            Debug.Log("Weapon Controller: No weapon found!");
        }
            
    }

    //U: Changes weapon, id is the position in weapons Hierarchy
    public void SwitchWeapon(int id)
    {
        Debug.Log("id:" + id.ToString());
        if (id >= 0 && id < weapons.Length)
        {
            Debug.Log("inside");
            for (int i = 0; i < weapons.Length; i++)
            {
                if(i == id)
                {
                    activeWeapon = weapons[i];
                    activeWeapon.Enable();
                }
                else
                {
                    weapons[i].Disable();
                }
            }
        }
    }

    public void RotateUpDown(float rotation)
    {
        transform.rotation *= Quaternion.Euler(-rotation, 0, 0);
    }

    //U: Points weapon in that direction
    public void PointAt(Vector3 target)
    {
        Vector3 origin = transform.position;
        Vector3 forward = (target - origin).normalized;
        Vector3 right = Vector3.Cross(forward, -Vector3.up);
        Vector3 upward = Vector3.Cross(forward, right);

        //Debug.DrawRay(origin, upward, Color.green); //up
        //Debug.DrawRay(origin, forward, Color.blue); // forward
        //Debug.DrawRay(origin, right, Color.red); // right

        transform.rotation = Quaternion.LookRotation(forward, upward);
    }

    //U: ShootS/AttackS if weapon is loaded/ready
    public Vector3 TryAction()
    {
        if (activeWeapon != null)
        {
            return activeWeapon.TryAction(bulletMechanic);
        }
        return Vector3.zero;
    }

    internal float GetFocusAngle()
    {
        return activeWeapon.focusAngle;
    }

    internal float GetFocusSpeed()
    {
        return activeWeapon.focusSpeed;
    }

    internal float GetFocusSensi()
    {
        return activeWeapon.focusSensibility;
    }
}
