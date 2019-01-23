using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSourceAlarm;
    public AudioSource audioSourceKlingel;

    private SerialCommunicator serialCommunicator;

    // Start is called before the first frame update
    void Start()
    {
        serialCommunicator = GetComponent<SerialCommunicator>();
        audioSourceAlarm.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        activateSound(serialCommunicator.klingelState, audioSourceKlingel);
        activateSound(serialCommunicator.isAlarm, audioSourceAlarm);
        deactivateSound(serialCommunicator.isAlarm, audioSourceAlarm);
    }

    private void deactivateSound(bool activate, AudioSource audio)
    {
        if (!activate && audio.isPlaying)
        {
            audio.Stop();
        }       
    }

    private void activateSound(bool activate, AudioSource audio)
    {
        if (activate && activate != audio.isPlaying)
        {
            audio.Play();
        }
    }
}
