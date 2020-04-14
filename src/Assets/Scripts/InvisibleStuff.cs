using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleStuff : MonoBehaviour
{
    public GameObject[] originalGOs;
    public GameObject[] transparentsGOs;
    public Transform player;


    bool opaque = true;
    // Update is called once per frame
    void Update()
    {
        if(player.position.z > 11f && player.position.z < 24f && opaque)
        {
            foreach (var item in originalGOs)
            {
                item.SetActive(false);
            }
            foreach (var item in transparentsGOs)
            {
                item.SetActive(true);
            }
            opaque = false;
        }
        else if ((player.position.z < 10f || player.position.z > 25f) && !opaque)
        {
            foreach (var item in originalGOs)
            {
                item.SetActive(true);
            }
            foreach (var item in transparentsGOs)
            {
                item.SetActive(false);
            }
            opaque = true;
        }
    }
}
