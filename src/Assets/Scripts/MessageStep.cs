using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageStep : Step
{
    [SerializeField]
    public string[] messages;

    private void OnEnable()
    {
        onActive.RemoveAllListeners();
        if (GetComponent<Collider>() == null)
            onActive.AddListener(() => Trigger(FindObjectOfType<PlayerFirstPersonInput>()));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            var fpsInput = other.GetComponentInParent<PlayerFirstPersonInput>();
            if (fpsInput)
            {
                Trigger(fpsInput);
            }
        }
    }

    private void Trigger(PlayerFirstPersonInput fpsInput)
    {
        DeactivateFPS(fpsInput);
        DialogCanvas.ShowDialog(messages, () => {
            ActivateFPS(fpsInput);
            completed = true;
            onCompleted.Invoke();
        });
    }

    public override bool IsCompleted()
    {
        return completed;
    }

    void DeactivateFPS(PlayerFirstPersonInput fpsInput)
    {
        fpsInput.enabled = false;
        fpsInput.GetComponent<UnitController>().enabled = false;
    }

    void ActivateFPS(PlayerFirstPersonInput fpsInput)
    {
        fpsInput.enabled = true;
        fpsInput.GetComponent<UnitController>().enabled = true;
    }

}
