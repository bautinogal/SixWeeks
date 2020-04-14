using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls Enemy movement, interaction and AI (Not very intelligent...)
public class Enemy : MonoBehaviour, IDamageable
{
    Vector3 originalPos;
    [SerializeField]
    float life = 10;
    [SerializeField]
    float amplitud = 1;
    [SerializeField]
    float frecuency = 1;
    [SerializeField]
    Vector3 direction = Vector3.right;
    [SerializeField]
    float damageAnimationDuration = 0.5f;
    [SerializeField]
    int damageAnimationCicles = 8;
    [SerializeField]
    int visionRadius = 8;

    float damageAnimationCounter = 0;

    WeaponController weaponController;
    Collider[] posibleTargetColliders = new Collider[4];

    public void OnDamage(float power)
    {
        life -= power;
        damageAnimationCounter = damageAnimationDuration;
        if (life < 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        weaponController = GetComponentInChildren<WeaponController>();
    }

    Transform LookForTarget()
    {
        Transform result = null;
        Physics.OverlapSphereNonAlloc(transform.position, visionRadius, posibleTargetColliders, 2 << 7, QueryTriggerInteraction.Collide);
        if (posibleTargetColliders[0])
        {
            result = posibleTargetColliders[0].transform;
            posibleTargetColliders[0] = null;
        }
        return result;
    }

    // Moves back and forth in a line
    void Update()
    {
        transform.localPosition = originalPos + direction * amplitud * Mathf.Sin(Time.time * frecuency);
        if(damageAnimationCounter > 0)
        {
            transform.localScale = Vector3.one * (1f + 0.2f * Mathf.Sin(Time.time * damageAnimationCicles));
        }
        else
        {
            transform.localScale = Vector3.one;
        }
        damageAnimationCounter -= Time.deltaTime;

        var target = LookForTarget();
        if (target && weaponController)
        {
            LookAt(target);
            WeaponAction();
        }
    }

    //U: Rotates the unit towards the target
    private void LookAt(Transform target)
    {
        Vector3 forward = Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    //U: Shoots or change weapon
    private void WeaponAction()
    {
        weaponController.TryAction();
    }
}
