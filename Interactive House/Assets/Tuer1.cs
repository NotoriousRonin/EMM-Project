using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuer1 : MonoBehaviour {

    private SerialCommunicator SerialCommunicator;
    public GameObject toHide;

    // Use this for initialization
    void Start () {
        SerialCommunicator = GameObject.Find("SerialCommunicator").GetComponent<SerialCommunicator>();
    }
	
	// Update is called once per frame
	void Update () {
        toHide.GetComponent<Renderer>().enabled = SerialCommunicator.sonarCM > 9 || SerialCommunicator.sonarCM < 5;
    }
}
