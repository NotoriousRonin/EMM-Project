using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    /// <summary>
    /// Source of Light
    /// </summary>
    public Light lightsource;

    /// <summary>
    /// Arduino Values
    /// </summary>
    private SerialCommunicator serialCommunicator;

    /// <summary>
    /// Base Lumen is set by the first LumenValue by the Arduino
    /// </summary>
    private int baseLumen;

    // Start is called before the first frame update
    void Start()
    {
        serialCommunicator = GetComponent<SerialCommunicator>();
        baseLumen = serialCommunicator.lichtsensorState;
    }

    // Update is called once per frame
    /// <summary>
    /// There is a new Lumen Value every frame coming in from the Arduino Sensor
    /// Every Lumen Value is a Percentage of the BaseLumen
    /// The Intensity of the Lightsource will be set to that Percantage
    /// </summary>
    void Update()
    {
        float percentage = ((float)serialCommunicator.lichtsensorState / (float)baseLumen);
        if (percentage > 3) percentage = 3;
        if (percentage < 0) percentage = 0;
        lightsource.intensity = Mathf.Round(percentage * 10) / 10;
    }
}