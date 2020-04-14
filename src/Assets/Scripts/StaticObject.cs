using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStopable
{
    void Stop(Collider col);
}

public class StaticObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        var other = col.GetComponentInParent<IStopable>();
        if (other != null)
        {
            other.Stop(GetComponent<Collider>());
        }
        Debug.Log("Choco");
    }
}
