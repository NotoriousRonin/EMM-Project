using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    /// <summary>
    /// AudioSource of the Alarm Sound
    /// Source: https://www.youtube.com/watch?v=PowGPSdAxTI
    /// </summary>
    public AudioSource audioSourceAlarm;

    /// <summary>
    /// AudioSource of the Doorbell
    /// Source: http://soundbible.com/1466-Doorbell.html
    /// </summary>
    public AudioSource audioSourceDoorbell;

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
        activateSound(serialCommunicator.klingelState, audioSourceDoorbell);
        //Would deactivate Sound everytime on multiple Pressed on the Button
        //Leaving it out means you have to wait till Sound is finished for it to be triggered again by pressing the Button on the Arduino
        //deactivateSound(serialCommunicator.klingelState, audioSourceDoorbell);
        activateSound(serialCommunicator.isAlarm, audioSourceAlarm);
        deactivateSound(serialCommunicator.isAlarm, audioSourceAlarm);
    }

    /// <summary>
    /// Deactivates a Audiosource Clip
    /// </summary>
    /// <param name="activate">Rather the Clip should be deactivated</param>
    /// <param name="audio">AudioSource to be deactivated</param>
    private void deactivateSound(bool activate, AudioSource audio)
    {
        if (!activate && audio.isPlaying)
        {
            audio.Stop();
        }       
    }

    /// <summary>
    /// Activates a Audiosource Clip
    /// </summary>
    /// <param name="activate">Rather the Clip should be deactivated</param>
    /// <param name="audio">AudioSource to be deactivated</param>
    private void activateSound(bool activate, AudioSource audio)
    {
        if (activate && activate != audio.isPlaying)
        {
            audio.Play();
        }
    }
}