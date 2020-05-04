using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerVolume : MonoBehaviour
{
    [SerializeField]
    EventTriggerVolume[] previousEvents;
    [SerializeField]
    EventTriggerVolume[] requiredEvents;

    [SerializeField]
    bool active = false;
    [SerializeField]
    bool completed = false;

    [SerializeField]
    string[] messages;

    public UnityEvent onDisable = new UnityEvent();
    public UnityEvent onActive = new UnityEvent();
    public UnityEvent onRead = new UnityEvent();
    public UnityEvent onCompleted = new UnityEvent();

    private void Start()
    {
        foreach (var item in requiredEvents)
        {
            onCompleted.AddListener(() => CheckCompletedCond());
        }
        if (previousEvents != null)
        {
            foreach (var item in previousEvents)
            {
                onCompleted.AddListener(() => CheckStartCond());
            }
            Disable();
            CheckStartCond();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            var fpsInput = other.GetComponentInParent<PlayerFirstPersonInput>();
            if (fpsInput)
            {
                if (true)
                {
                    active = false;
                    completed = true;
                }
                DeactivateFPS(fpsInput);
                DialogCanvas.ShowDialog(messages, () => {
                    ActivateFPS(fpsInput);
                    CheckCompletedCond();
                    }
                );
            }
        }
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

    void CheckStartCond()
    {
        if(previousEvents != null)
        {
            foreach (var item in previousEvents)
            {
                if (!item.IsCompleted())
                    return;
            }
        }
        SetActive();
    }

    void CheckCompletedCond()
    {
        if(requiredEvents != null)
        {
            foreach (var item in requiredEvents)
            {
                if (!item.IsCompleted())
                    return;
            }
        }
        Completed();
    }

    private void Disable()
    {
        onDisable.Invoke();
        gameObject.SetActive(false);
    }

    private void SetActive()
    {
        gameObject.SetActive(true);
        onActive.Invoke();
    }

    private void Completed()
    {
        onCompleted.Invoke();
        Disable();
    }

    public bool IsActive()
    {
        return active;
    }

    public bool IsCompleted()
    {
        return completed;
    }

}
