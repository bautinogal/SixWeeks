using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabStep : Step
{
    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            completed = true;
            active = false;
            onCompleted.Invoke();
            gameObject.SetActive(false);
        }
    }

    public override bool IsCompleted()
    {
        return completed;
    }
}
