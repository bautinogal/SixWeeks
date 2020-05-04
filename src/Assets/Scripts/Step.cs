using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Step : MonoBehaviour
{
    [SerializeField]
    protected Step[] previousSteps;

    [SerializeField]
    protected bool active = false;
    [SerializeField]
    protected bool completed = false;

    public UnityEvent onDisable = new UnityEvent();
    public UnityEvent onActive = new UnityEvent();
    public UnityEvent onCompleted = new UnityEvent();

    private void Start()
    {
        if (previousSteps != null)
        {
            foreach (var step in previousSteps)
            {
                step.onCompleted.AddListener(() => CheckifReady());
            }
        }
        CheckifReady();
    }

    public virtual Step[] RequiredSteps()
    {
        if (previousSteps == null)
            return new Step[0];
        else
            return previousSteps;
    } 

    public virtual bool IsCompleted()
    {
        if (completed == false && active)
        {
            completed = true;
            onCompleted.Invoke();
        }
        return completed;
    }

    public virtual void CheckifReady()
    {
        var ready = true;
        if(previousSteps != null)
        {
            foreach (var step in previousSteps)
            {
                if (!step.IsCompleted())
                    ready = false;
            }
        }
        if (ready)
            Activate();
    }

    public virtual void Activate()
    {
        active = true;
        onActive.Invoke();
    }
}
