using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawProyectilePath : MonoBehaviour
{
    WeaponController weaponController;
    LineRenderer lineRenderer;

    private void Start()
    {
        weaponController = GetComponentInChildren<WeaponController>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        Keyframe start = new Keyframe(0, 0.15f);
        Keyframe end = new Keyframe(1, 0.15f);
        lineRenderer.widthCurve = new AnimationCurve(start, end);
    }

    private void Update()
    {
        
        lineRenderer.SetPosition(0, weaponController.transform.position);
        lineRenderer.SetPosition(1, weaponController.transform.position + weaponController.transform.forward * 100);
    }
}
