using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFlare : MonoBehaviour
{
    [SerializeField]
    float duration = 0.3f;

    public void Show()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowCo());
    }

    IEnumerator ShowCo()
    {
        var counter = 0f;
        while (counter < 1f)
        {
            counter += Time.deltaTime / duration;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
