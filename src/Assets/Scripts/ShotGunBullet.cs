using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunBullet : MonoBehaviour
{
    bool active;
    float damage = 0f;
    float lifeSpan = 1f;
    float life = 0;
    float speed = 1f;
    Vector3 dir;
    Transform pistolTrans;

    public void SetParent(Transform parent)
    {
        pistolTrans = parent;
    }

    public bool IsActive()
    {
        return active;
    }

    public void Sleep()
    {
        transform.parent = pistolTrans;
        gameObject.SetActive(false);
        active = false;
    }

    public void Shoot(Vector3 origin, Vector3 direction,float damage, float speed, float lifeSpam)
    {
        this.damage = damage;
        this.speed = speed;
        this.lifeSpan = lifeSpam;
        transform.position = origin;
        dir = direction;
        life = 0;
        transform.parent = null;
        gameObject.SetActive(true);
        active = true;
    }

    private void Update()
    {
        transform.position += dir * speed * 0.5f * (1 + lifeSpan - life) * Time.deltaTime;
        life += Time.deltaTime;
        if (life > lifeSpan)
            Sleep();
    }

    private void OnTriggerEnter(Collider col)
    {
        var other = col.GetComponentInParent<IDamageable>();
        if (other != null)
        {
            other.OnDamage(damage);
        }
        Sleep();
    }

}
