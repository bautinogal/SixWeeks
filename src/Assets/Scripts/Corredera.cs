using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corredera : MonoBehaviour
{
    ShotGun shotGun;
    AnimationCurve curve = new AnimationCurve();
    [SerializeField]
    Vector2[] keyframesVect;
    Vector3 originalPos;

    private void Start()
    {
        shotGun = GetComponentInParent<ShotGun>();
        originalPos = transform.localPosition;
        var keyframes = new Keyframe[keyframesVect.Length];
        for (int i = 0; i < keyframesVect.Length; i++)
        {
            keyframes[i] = new Keyframe(keyframesVect[i].x, keyframesVect[i].y);
        }
        curve.keys = keyframes;
    }


    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(PlayAnimCo());
    }

    IEnumerator PlayAnimCo()
    {
        float counter = 0;
        float duration = shotGun.reloadTime;
        transform.localPosition = originalPos;
        while (counter < 1f)
        {
            var pos = new Vector3(originalPos.x, originalPos.y, curve.Evaluate(counter));
            transform.localPosition = pos;
            counter += Time.deltaTime / shotGun.reloadTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
