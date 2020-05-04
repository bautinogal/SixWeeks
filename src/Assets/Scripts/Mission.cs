using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
    [SerializeField]
    Mission[] previousMissions;
    [SerializeField]
    Step[] steps;

    [SerializeField]
    bool active = false;
    [SerializeField]
    bool completed = false;

    public UnityEvent onActive = new UnityEvent();
    public UnityEvent onInactive = new UnityEvent();
    public UnityEvent onCompleted = new UnityEvent();

    private void Start()
    {
        GetStepsInChildrens();
        SuscribeSteps();
        if (MissionReady() && !completed)
        {
            SetActive();
            UpdateMission();
        }
        else
            SetInactive();
    }

    private void GetStepsInChildrens()
    {
        var childSteps = transform.GetComponentsInChildren<Step>();
        steps = childSteps;
    }

    private void SuscribeSteps()
    {
        if(steps != null)
        {
            foreach (var step in steps)
            {
                step.onCompleted.AddListener(() => UpdateMission());
            }
        }
    }

    private bool MissionReady()
    {
        if (previousMissions != null)
        {
            foreach (var mission in previousMissions)
            {
                if (mission.IsCompleted())
                    return false;
            }
        }
        return true;
    }

    private void UpdateMission()
    {
        var missionCompleted = true;
        if (steps != null)
        {
            foreach (var step in steps)
            {
                if (!step.IsCompleted())
                {
                    missionCompleted = false;
                }
            }
        }
        if(missionCompleted)
            Completed();
    }

    public bool IsCompleted()
    {
        return completed;
    }

    private void Completed()
    {
        completed = true;
        onCompleted.Invoke();
        SetInactive();
    }

    private void SetActive()
    {
        active = true;
        gameObject.SetActive(true);
        onActive.Invoke();
    }
    
    private void SetInactive()
    {
        active = false;
        onInactive.Invoke();
        gameObject.SetActive(false);
    }
}
