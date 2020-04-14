using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : Weapon
{
    [SerializeField]
    float damage = 1f;
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    float lifeSpan = 3f;
    //Min time between shots
    [SerializeField]
    float reloadTime = 1f;
    //Time until next shot is ready
    float coolDown;

    //Bullets pool (for perfonmance reasons it is better to have a pool of objects instead of instancing)
    SpitterBullet[] bullets;

    void Start()
    {
        bullets = GetComponentsInChildren<SpitterBullet>();
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetParent(transform);
            bullets[i].Sleep();
        }

    }

    private void Update()
    {
        coolDown = Mathf.Clamp(coolDown - Time.deltaTime, -1f, reloadTime);
    }

    private SpitterBullet GetIdleBullet()
    {
        SpitterBullet result = null;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].IsActive())
            {
                result = bullets[i];
                return result;
            }
        }
        return result;

    }

    public override Vector3 TryAction(WeaponController.BulletMechanic bulletMechanic)
    {
        if (coolDown <= 0f)
        {
            var idleBullet = GetIdleBullet();
            if (idleBullet != null)
            {
                idleBullet.Shoot(transform.position, transform.forward, damage, speed, lifeSpan);
                coolDown = reloadTime;
                return Vector3.zero;
            }
            else
            {
                Debug.Log("Bullet pool is empty!");
                return Vector3.zero;
            }
        }
        return Vector3.zero;
    }
}
