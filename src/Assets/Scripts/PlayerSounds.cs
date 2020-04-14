using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{


    IEnumerator PlayStepsCo(float clipLenght)
    {
        WaitForSeconds wait = new WaitForSeconds(clipLenght);
        while (true)
        {
            //stepsAudioSource.volume = (0.8f + Random.Range(-0.4f, 0.1f)) * targetVolume;
            //stepsAudioSource.pitch = 1f + Random.Range(-0.5f, 0.5f);
            yield return wait;
        }
    }
}
