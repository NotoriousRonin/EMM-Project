﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour {

    private SerialCommunicator SerialCommunicator;
    public Animator animatorHouse;
    public GameObject kaminfeuer;

    void Start()
    {
        SerialCommunicator = GameObject.Find("Controller").GetComponent<SerialCommunicator>();
    }
    // Update is called once per frame
    void Update()
    {

        if (SerialCommunicator.magnetState == true)
        {
            animatorHouse.SetBool("magnetState", true);
            kaminfeuer.SetActive(true);
        }
        else
        {
            animatorHouse.SetBool("magnetState", false);
            kaminfeuer.SetActive(false);
        }

    }

}
