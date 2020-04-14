using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    float maxLife = 100;
    [SerializeField]
    float life = 100;
    [SerializeField]
    Transform playerModel;

    [SerializeField]
    float damageAnimationDuration = 0.5f;
    [SerializeField]
    int damageAnimationCicles = 8;

    float damageAnimationCounter = 0;

    private void OnEnable()
    {
        life = maxLife;
    }

    public virtual void OnDamage(float power)
    {
        life -= power;
        damageAnimationCounter = damageAnimationDuration;
        HealthGUI.SetPercentage(life / maxLife);

        if (life < 0)
        {
            EndGameMenu.Defeat();
            gameObject.SetActive(false);
        }
    }

    // Animation when unit is hit
    void Update()
    {
        if (damageAnimationCounter > 0)
        {
            playerModel.localScale = Vector3.one * (1f + 0.2f * Mathf.Sin(Time.time * damageAnimationCicles));
        }
        else
        {
            playerModel.localScale = Vector3.one;
        }
        damageAnimationCounter -= Time.deltaTime;

        if (damageAnimationCounter < 0)
        {
            damageAnimationCounter = 0;
        }
    }


}
