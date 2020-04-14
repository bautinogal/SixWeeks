using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Weapon
{
    [SerializeField]
    float damage = 3f;
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    float lifeSpan = 3f;

    //Min time between shots
    [SerializeField]
    public float reloadTime = 1f;
    [SerializeField]
    int bulletsPerShot = 6;

    [Header("AngleSpread")]
    [SerializeField]
    [Range(0.01f,3f)]
    float angleHorizontalSpread = 0.1f;
    [SerializeField]
    [Range(0.01f, 3f)]
    float angleVerticalSpread = 0.1f;

    [Header("Pos Spread")]
    [SerializeField]
    [Range(0.01f, 3f)]
    float posHorizontalSpread = 0.1f;
    [SerializeField]
    [Range(0.01f, 3f)]
    float posVerticalSpread = 0.1f;
    [SerializeField]
    [Range(0.01f, 3f)]
    float posForwardSpread = 0.1f;

    //Time until next shot is ready
    float coolDown;

    //Bullets pool (for perfonmance reasons it is better to have a pool of objects instead of instancing)
    ShotGunBullet[] bullets;
    AudioSource audioSource;
    Corredera corredera;

    [SerializeField]
    Transform spawnPos;
    [SerializeField]
    string[] collisionsLayers;
    int colMask;

    [SerializeField]
    float recoil = 1f;


    void Start()
    {
        bullets = GetComponentsInChildren<ShotGunBullet>();
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetParent(transform);
            bullets[i].Sleep();
        }
        audioSource = GetComponent<AudioSource>();
        corredera = GetComponentInChildren<Corredera>();
        colMask = LayerMask.GetMask(collisionsLayers);
    }

    private void Update()
    {
        coolDown = Mathf.Clamp(coolDown - Time.deltaTime, -1f, reloadTime);
    }

    private ShotGunBullet[] GetIdleBullets()
    {
        var counter = 0;
        ShotGunBullet[] result = new ShotGunBullet[bulletsPerShot];
        for (int i = 0; i < bullets.Length && counter < bulletsPerShot; i++)
        {
            if (!bullets[i].IsActive())
            {
                result[counter] = bullets[i];
                counter++;
            }
        }
        if (result.Length == bulletsPerShot)
            return result;
        else
        {
            result = null;
            return result;
        }
    }

    public override Vector3 TryAction(WeaponController.BulletMechanic bulletMechanic)
    {
        if (coolDown <= 0f)
        {
            var idleBullets = GetIdleBullets();
            if (idleBullets != null)
            {
                if(bulletMechanic == WeaponController.BulletMechanic.OnTrigger)
                {
                    for (int i = 0; i < idleBullets.Length; i++)
                    {
                        var pos = transform.position +
                            transform.right * Random.Range(-posHorizontalSpread, posHorizontalSpread) +
                            transform.up * Random.Range(-posVerticalSpread, posVerticalSpread) +
                            transform.forward * Random.Range(-posForwardSpread, posForwardSpread);
                        var dir = transform.forward +
                            transform.up * Random.Range(-angleVerticalSpread, angleVerticalSpread) +
                            transform.right * Random.Range(-angleHorizontalSpread, angleHorizontalSpread);
                        idleBullets[i].Shoot(pos, dir, damage, speed, lifeSpan);
                        coolDown = reloadTime;
                    }
                }
                else if (bulletMechanic == WeaponController.BulletMechanic.Raycast)
                {
                    for (int i = 0; i < idleBullets.Length; i++)
                    {
                        var pos = spawnPos.position +
                            transform.right * Random.Range(-posHorizontalSpread, posHorizontalSpread) +
                            transform.up * Random.Range(-posVerticalSpread, posVerticalSpread) +
                            transform.forward * Random.Range(-posForwardSpread, posForwardSpread);
                        var dir = spawnPos.forward +
                            transform.up * Random.Range(-angleVerticalSpread, angleVerticalSpread) +
                            transform.right * Random.Range(-angleHorizontalSpread, angleHorizontalSpread);

                        var ray = new Ray(pos, dir);
                        var hitInfo = new RaycastHit();
                        if (Physics.Raycast(ray, out hitInfo, 200, colMask))
                        {
                            Debug.Log("Raycast hit shotgun: " + hitInfo.collider.gameObject.name);
                            idleBullets[i].Shoot(hitInfo.point, dir, damage, 0.1f, lifeSpan);
                            coolDown = reloadTime;
                        }
                        else
                        {
                            idleBullets[i].Sleep();
                        }
                    }
                }
                if(corredera)
                    corredera.Play();
                audioSource.Play();
                return Vector3.up * 2f;
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
