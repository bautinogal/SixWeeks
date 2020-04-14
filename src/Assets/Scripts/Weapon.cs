using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float focusAngle = 30f;
    public float focusSpeed = 0.3f;
    public float focusSensibility = 0.3f;

    public virtual Vector3 TryAction(WeaponController.BulletMechanic bulletMechanic)
    {
        //This vector represents weapon recoil
        return Vector3.zero;
    }

    public virtual void Enable()
    {
        gameObject.SetActive(true);
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}
