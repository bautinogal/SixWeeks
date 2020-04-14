using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    AudioSource tranca;
    [SerializeField]
    AudioSource accion;

    [SerializeField][Range(0,1f)]
    float volume = 1f;
    [SerializeField]
    float zMin = 10f;
    [SerializeField]
    float zMax = 40f;

    // Update is called once per frame
    void Update()
    {
        if (player.position.z < zMin)
        {
            tranca.volume = volume;
            accion.volume = 0;
        }
        else if (player.position.z < zMax)
        {
            float interpol = Mathf.SmoothStep(0,1,(player.position.z - zMin) / (zMax - zMin));
            tranca.volume = Mathf.Pow(1f - interpol, 2) * volume;
            accion.volume = Mathf.Pow(interpol, 3) * volume;
        }
        else
        {
            tranca.volume = 0;
            accion.volume = volume;
        }
    }
}
