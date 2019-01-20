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
        decimal res = ((decimal)SerialCommunicator.potiTuer).Map( SerialCommunicator.potiTuerMin, SerialCommunicator.potiTuerMax, 0, 90);
        Debug.Log(SerialCommunicator.potiTuer + " to: " + res);
        Quaternion target = Quaternion.Euler(0, (int)res, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
