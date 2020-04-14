using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//U: Class that controls camera movement and propertys
public class GameCamera : MonoBehaviour
{
    //TODO: Move config data to json or similar:
    //--------------------------------------------------------------------------------
    // Camera configuration for CLOSEUP
    [Header("CloseUp")]
    [SerializeField]
    float closeSize;
    [SerializeField]
    Vector3 closePos;
    [SerializeField]
    Quaternion closeRot;

    // Camera configuration for STANDARD
    [Header("Isometric")]
    [SerializeField]
    float standardSize;
    [SerializeField]
    Vector3 standardPos;
    [SerializeField]
    Quaternion standardRot;

    // Other Camera configuration settings
    [Header("Other Config")]
    [SerializeField]
    float transDuration;
    //--------------------------------------------------------------------------------

    //Class vars:
    static GameCamera instance;
    static Transform camTrans;
    static Camera cam;
    static Transform target;
    public enum CamState {STANDARD, CLOSEUP, CUSTOM, TRANSITION}
    static CamState state;
    static Vector3 actualOffset;

    //Singleton Pattern:
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            camTrans = transform;
            cam = GetComponent<Camera>();
        }
        else
            Destroy(this);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Q))
            actualOffset = Quaternion.Euler(0, Time.deltaTime * 40, 0) * actualOffset;
        if(Input.GetKey(KeyCode.E))
            actualOffset = Quaternion.Euler(0, -Time.deltaTime * 40, 0) * actualOffset;
    }

    //Returns Camera state
    public static CamState GetState()
    {
        return state;
    }

    //U: Sets camera target (player generally)
    public static void SetTarget(Transform targ)
    {
        target = targ;
    }

    //U: Interpolates smoothly to custom position, rotation and size
    public static void GoToCustom(Transform target, Vector3 offset, float size, float duration, Action cb = null)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(GoToCo(target, offset, size, duration, CamState.CUSTOM, cb));
    }

    //U: Interpolates smoothly to "Close Up" postion, rotation and size
    public static void GoToCloseUp(float duration, Action cb = null)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(GoToCo(target, instance.closePos, instance.closeSize, duration, CamState.CLOSEUP, cb));
    }

    public static void GoToCloseUp(Action cb = null)
    {
        GoToCloseUp(instance.transDuration, cb);
    }

    //U: Interpolates smoothly to "Standard" postion, rotation and size
    public static void GoToStandard(float duration, Action cb = null)
    {
        instance.StopAllCoroutines();
        instance.StartCoroutine(GoToCo(target, instance.standardPos, instance.standardSize, duration, CamState.STANDARD, cb));
    }

    public static void GoToStandard(Action cb = null)
    {
        GoToStandard(instance.transDuration, cb);
    }
    
    //U: Coroutine that interpolates to final pos, rot and size
    static IEnumerator GoToCo(Transform target, Vector3 offset, float size, float duration, CamState finalState, Action cb)
    {
        float counter = 0;
        float smoothedCounter = 0;
        float speed = 1f / duration;

        Vector3 initialOffset = camTrans.position - GameCamera.target.position;
        Quaternion initialRot = camTrans.rotation;
        float initialsSize = cam.orthographicSize;
        actualOffset = offset;

        camTrans.position = GameCamera.target.position + offset;
        Vector3 forward = target.position + Vector3.up - camTrans.position;
        Vector3 right = Vector3.Cross(forward, Vector3.up);
        Vector3 up = Vector3.Cross(right, forward);
        Quaternion finalRot = Quaternion.LookRotation(forward, up);
        camTrans.position = initialOffset + GameCamera.target.position;

        state = CamState.TRANSITION;
        //A: Smooth interpolation using smoothstep function
        while (counter < 1f)
        {
            counter += Time.deltaTime * speed;
            smoothedCounter = Mathf.SmoothStep(0, 1, counter);

            camTrans.position = GameCamera.target.position + Vector3.Lerp(initialOffset, actualOffset, smoothedCounter);
            camTrans.rotation = Quaternion.Lerp(initialRot, finalRot, smoothedCounter);
            cam.orthographicSize = Mathf.Lerp(initialsSize, size, smoothedCounter);
            yield return null;
        }

        //A: Camera reachs final position, rotation and size:
        state = finalState;
        if (cb != null)
            cb();

        //Repeat once per frame until a new "GoTo()" is commanded:
        while (true)
        {
            if (!PauseMenu.IsPaused())
            {
                camTrans.localPosition = GameCamera.target.position + actualOffset;
                forward = target.position + Vector3.up - camTrans.position;
                right = Vector3.Cross(forward, Vector3.up);
                up = Vector3.Cross(right, forward);
                camTrans.rotation = Quaternion.LookRotation(forward, up);
                cam.orthographicSize = size;
            }
            
            yield return null;
        }
    }
        
}
