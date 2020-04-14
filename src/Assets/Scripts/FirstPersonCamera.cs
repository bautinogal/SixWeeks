using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    Transform player;

    [SerializeField]
    Vector3 offset;

    WeaponController weaponController;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerFirstPersonInput>().transform;
        weaponController = player.GetComponentInChildren<WeaponController>();
        transform.position = weaponController.transform.position + weaponController.transform.up * .5f + weaponController.transform.forward * -.8f;
        transform.rotation = weaponController.transform.rotation;
        transform.parent = weaponController.transform;
        cam = GetComponent<Camera>();
    }

    public void Focus(float angle, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(ZoomCo(angle,speed));
    }

    IEnumerator ZoomCo(float angle, float speed)
    {
        float initialAngle = cam.fieldOfView;
        float currentAngle = initialAngle;
        bool ready = false;
        if(angle > initialAngle)
        {
            while (!ready)
            {
                currentAngle += speed * 45f * Time.deltaTime;
                if (currentAngle > angle)
                {
                    currentAngle = angle;
                    cam.fieldOfView = currentAngle;
                    ready = true;
                }
                else
                {
                    cam.fieldOfView = currentAngle;
                }
                yield return null;
            }
        }
        else
        {
            while (!ready)
            {
                currentAngle -= speed * 45f * Time.deltaTime;
                if (currentAngle < angle)
                {
                    currentAngle = angle;
                    cam.fieldOfView = currentAngle;
                    ready = true;
                }
                else
                {
                    cam.fieldOfView = currentAngle;
                }
                yield return null;
            }
        }
    }

}
