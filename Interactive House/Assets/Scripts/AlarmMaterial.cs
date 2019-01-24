using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmMaterial : MonoBehaviour
{
    private SerialCommunicator SerialCommunicator;

    public Material matRed;
    public Material matNormal;
    public Light light;
    private float t = 0;


    void Start()
    {
        SerialCommunicator = GameObject.Find("Controller").GetComponent<SerialCommunicator>();

        
    }
    // Update is called once per frame
    void Update()
    {

        if (SerialCommunicator.isAlarm == true)
        {
            changeBetweenMaterials();
        }
        else
        {
            changeMaterial();
        }



    }

    private void changeBetweenMaterials()
    {
        Renderer  renderer= this.gameObject.GetComponent<Renderer>();
        renderer.material = matRed;
        light.intensity = 10;

        //WaitForSeconds(0.25)??
        /*
        renderer.material = matNormal;
        light.intensity = 0;
        */
    }


    private void changeMaterial()
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.material = matNormal;
        light.intensity = 0;
    }

}
