using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie0 : MonoBehaviour, IDamageable
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

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
    }

    public void OnDamage(float power)
    {
        life -= power;
        damageAnimationCounter = damageAnimationDuration;
        if (life < 0)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (damageAnimationCounter > 0)
        {
            transform.localScale = Vector3.one * (1f + 0.2f * Mathf.Sin(Time.time * damageAnimationCicles));
        }
        else
        {
            transform.localScale = Vector3.one;
        }
        damageAnimationCounter -= Time.deltaTime;
    }
}
