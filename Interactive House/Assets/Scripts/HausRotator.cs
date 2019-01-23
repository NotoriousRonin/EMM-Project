using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HausRotator : MonoBehaviour {

    public Vector3 initGyroRotation = new Vector3(356, 352, 201); // in degree
    private SerialCommunicator serialCommunicator;

	// Use this for initialization
	void Start () {
        serialCommunicator = GameObject.Find("SerialCommunicator").GetComponent<SerialCommunicator>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 newRotation = initGyroRotation - new Vector3(serialCommunicator.GyroX, serialCommunicator.GyroY, serialCommunicator.GyroZ);
        // ignore y and z for now
        //newRotation.y = 0f;
        newRotation.z = serialCommunicator.GyroY;
        newRotation.y = 0;
        //newRotation.y = serialCommunicator.GyroZ;
        transform.rotation = Quaternion.Euler(newRotation);
    }
}