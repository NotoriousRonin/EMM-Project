using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorRotator : MonoBehaviour {
    private SerialCommunicator SerialCommunicator;
    float smooth = 5.0f;

    // Use this for initialization
    void Start () {
        SerialCommunicator = GameObject.Find("SerialCommunicator").GetComponent<SerialCommunicator>();
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion target = Quaternion.Euler(0, SerialCommunicator.potiTuer * 60f, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
        //transform.rotation.y = SerialCommunicator.potiTuer;
        //transform.Rotate(0, SerialCommunicator.potiTuer, 0, Space.Self);
    }
}
