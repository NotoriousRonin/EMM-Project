using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light lightsource;

    private SerialCommunicator serialCommunicator;

    private int baseLumen;

    // Start is called before the first frame update
    void Start()
    {
        serialCommunicator = GetComponent<SerialCommunicator>();
        baseLumen = serialCommunicator.lichtsensorState;
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = ((float)serialCommunicator.lichtsensorState / (float)baseLumen);
        if (percentage > 3) percentage = 3;
        if (percentage < 0) percentage = 0;
        lightsource.intensity = Mathf.Round(percentage * 10) / 10;
    }
}
