using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmMaterial : MonoBehaviour
{
    private SerialCommunicator SerialCommunicator;

    public Material matRed;
    public Material matNormal;
    public Light light;
    float nextUpdate = 0.3f;
    bool on = false;


    void Start()
    {
        SerialCommunicator = GameObject.Find("Controller").GetComponent<SerialCommunicator>();


    }

    // Update is called once per frame
    void Update()
    {

        if (SerialCommunicator.isAlarm == true)
        {

            if (Time.time >= nextUpdate)
            {
                nextUpdate = Time.time + 0.3f;
                on = !on;
                toggle();
            }



        }
        else
        {
            changeMaterial();
        }



    }


    private void toggle()
    {

        if (!on)
        {
            Renderer renderer = this.gameObject.GetComponent<Renderer>();
            renderer = this.gameObject.GetComponent<Renderer>();
            renderer.material = matRed;
            light.intensity = 10;
        }

         if (on) {
            Renderer renderer = this.gameObject.GetComponent<Renderer>();
            renderer = this.gameObject.GetComponent<Renderer>();
            renderer.material = matNormal;
            light.intensity = 0;
         }
    }

    private void changeMaterial()
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.material = matNormal;
        light.intensity = 0;
    }

}