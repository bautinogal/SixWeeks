using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRootCorrector : MonoBehaviour
{
    [SerializeField]
    Transform hips;

    [SerializeField]
    Vector3 pos;

    [SerializeField]
    [Range(0.01f, 1f)]
    float lerp = 0.3f;
    [SerializeField]
    [Range(0.1f, 5f)]
    float corrFact = 1f;

    private void Start()
    {
        pos = transform.position;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (hips != null)
            transform.position = Vector3.Lerp(transform.position, pos - corrFact * Vector3.ProjectOnPlane(hips.position - pos, Vector3.up), lerp);
    }
}
