using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectsController : MonoBehaviour
{
    private void Start()
    {
        Collider[] cols = GetComponentsInChildren<Collider>();
        Rigidbody[] rbs = new Rigidbody[cols.Length];
        StaticObject[] sos = new StaticObject[cols.Length];

        foreach (var col in cols)
        {
            var rb = col.GetComponent<Rigidbody>();
            if (rb == null)
                rb = col.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    
}
