using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorRotator : MonoBehaviour {
    private SerialCommunicator SerialCommunicator;
    float smooth = 5.0f;
    float angle = 0f;
    // Use this for initialization
    void Start () {
        SerialCommunicator = GameObject.Find("SerialCommunicator").GetComponent<SerialCommunicator>();
	}
	
	// Update is called once per frame
	void Update () {
        //decimal res = ((decimal)SerialCommunicator.potiTuer).Map( SerialCommunicator.potiTuerMin, SerialCommunicator.potiTuerMax, 0, 90);
        //Debug.Log(SerialCommunicator.potiTuer + " to: " + res);
        
        if(SerialCommunicator.potiTuer >= SerialCommunicator.potiTuerZu)
        {
            angle = 0f;
        }
        else if(SerialCommunicator.potiTuer <= SerialCommunicator.potiTuerOffen)
        {
            angle = 90f;
        }
        
        Quaternion target = Quaternion.Euler(0, (int)angle, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);
    }
}
