using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerVolume : MonoBehaviour
{
    [SerializeField]
    bool oneTime = true;
    bool active = true;

    [SerializeField]
    string[] messages;

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            var fpsInput = other.GetComponentInParent<PlayerFirstPersonInput>();
            if (fpsInput)
            {
                if (oneTime)
                    active = false;
                Deactivate(fpsInput);
                DialogCanvas.ShowDialog(messages, () => Activate(fpsInput));
            }
        }
        
    }

    void Deactivate(PlayerFirstPersonInput fpsInput)
    {
        fpsInput.enabled = false;
        fpsInput.GetComponent<UnitController>().enabled = false;
    }

    void Activate(PlayerFirstPersonInput fpsInput)
    {
        fpsInput.enabled = true;
        fpsInput.GetComponent<UnitController>().enabled = true;
    }
}
