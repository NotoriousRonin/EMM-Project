using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HausRotator : MonoBehaviour {

    
    private SerialCommunicator serialCommunicator;
    float smooth = 5.0f;

	// Use this for initialization
	void Start () {
        serialCommunicator = GameObject.Find("Controller").GetComponent<SerialCommunicator>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 newRotation = serialCommunicator.initGyroRotation - new Vector3(serialCommunicator.GyroX, serialCommunicator.GyroY, serialCommunicator.GyroZ);
        // ignore y and z for now
        newRotation.z = serialCommunicator.GyroY;
        newRotation.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime * smooth);
    }
}