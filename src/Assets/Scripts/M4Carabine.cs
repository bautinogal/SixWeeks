using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M4Carabine : Weapon
{
    [SerializeField]
    GameObject amunition;
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
    OtanStdAmo[] bullets;
    AudioSource audioSource;

    [SerializeField]
    Transform spawnPos;
    [SerializeField]
    string[] collisionsLayers;
    int colMask;

    [SerializeField]
    float recoil = 1f;

    void Start()
    {
        bullets = amunition.GetComponentsInChildren<OtanStdAmo>();
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetParent(amunition.transform);
            bullets[i].Sleep();
        }
        audioSource = GetComponent<AudioSource>();
        colMask = LayerMask.GetMask(collisionsLayers);
    }

    private void Update()
    {
        coolDown = Mathf.Clamp(coolDown - Time.deltaTime, -1f, reloadTime);
    }

    private OtanStdAmo GetIdleBullet()
    {
        OtanStdAmo result = null;
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
                if (bulletMechanic == WeaponController.BulletMechanic.OnTrigger)
                {
                    idleBullet.Shoot(spawnPos.position, spawnPos.forward, damage, speed, lifeSpan);
                }
                else if (bulletMechanic == WeaponController.BulletMechanic.Raycast)
                {
                    var ray = new Ray(spawnPos.position, spawnPos.forward);
                    var hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, 200, colMask))
                    {
                        Debug.Log("Raycast hit bullet: " + hitInfo.collider.gameObject.name);
                        idleBullet.Shoot(hitInfo.point, spawnPos.forward, damage, .1f, lifeSpan);
                        coolDown = reloadTime;
                    }
                    else
                    {
                        idleBullet.Sleep();
                    }
                }

                coolDown = reloadTime;
                audioSource.Play();
                return Vector3.up * recoil;
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
