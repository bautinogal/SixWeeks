using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls hearts displays and animtations at top left corner of the screen
public class HealthGUI : MonoBehaviour
{
    static HealthGUI instance;
    [SerializeField]
    Transform[] hearts;
    [SerializeField]
    Image background;
    [SerializeField]
    Image healthBar;

    float healthTarget = 250f;

    //Singleton Pattern
    void Start()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
            foreach(var heart in hearts)
            {
                heart.localScale = Vector3.zero;
            }
            background.color = Color.clear;
            healthBar.rectTransform.sizeDelta = new Vector2(0,40);
            Enable();
        }
    }

    //U: Plays animation
    static public void Enable(Action cb = null)
    {
        instance.StartCoroutine(instance.EnableCo(0,cb));
    }

    //U: Hides hearts
    static public void Disable()
    {
        instance.StopAllCoroutines();
        instance.background.color = Color.clear;
        instance.healthBar.rectTransform.sizeDelta = new Vector2(0, 40);
        foreach (var heart in instance.hearts)
        {
            heart.transform.localScale = Vector3.zero;
        }
    }

    //U: Scales health bar
    static public void SetPercentage(float percentage)
    {
        percentage = Mathf.Clamp01(percentage);
        instance.healthTarget = 250f * percentage;
        instance.healthBar.rectTransform.sizeDelta = new Vector2(instance.healthTarget, 40);
    }

    //U: Plays animation
    IEnumerator EnableCo(float delay = 0, Action cb = null)
    {
        float heartSpeed = 3;
        float barSpeed = 1;
        float counter = 0;
        yield return new WaitForSeconds(delay);
        while(counter < hearts.Length + 1)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                float interpol = counter - i;
                float val;
                if(interpol > 0)
                {
                    if(interpol < 1f)
                    {
                        val = Mathf.SmoothStep(0,1.3f, interpol);
                    }
                    else if (interpol < 1.5f)
                    {
                        val = Mathf.SmoothStep(1.3f, 1f, (interpol - 1f) * 2f);
                    }
                    else
                    {
                        val = 1f;
                    }
                    hearts[i].localScale = Vector3.one * val;
                }
            }

            
            counter += Time.deltaTime * heartSpeed;
            yield return null;
        }

        counter = 0;
        while (counter < 1f )
        {
            var val = Mathf.SmoothStep(0, 1f, counter);
            background.color = Color.Lerp(Color.clear, Color.black, val);
            healthBar.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(0, 40), new Vector2(healthTarget, 40), val);
            counter += Time.deltaTime * barSpeed;
            yield return null;
        }

        if (cb != null)
            cb();
    }
}
