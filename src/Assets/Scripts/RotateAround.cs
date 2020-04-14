using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField]
    Transform pivot;

    [SerializeField]
    float speed = 10f;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(pivot.position, transform.up, Time.deltaTime * speed);
    }
}
