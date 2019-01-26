using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BilderrahmenRotator : MonoBehaviour
{
    public float zClampMin;
    public float zClampMax;

    public float smooth = 5.0f;

    public float x;
    public float y;
    public float z;

    private SerialCommunicator serialCommunicator;

    public Vector3 calculatedRotation = new Vector3();
    public bool useCalculatedRotation = true;
    public bool useClamp = true;

    // Use this for initialization
    void Start()
    {
        serialCommunicator = GameObject.Find("Controller").GetComponent<SerialCommunicator>();
       // zClampMin = -4;
       // zClampMax = 4;
    }

    // Update is called once per frame
    void Update()
    {
        calculatedRotation = serialCommunicator.initGyroRotation - new Vector3(serialCommunicator.GyroX, serialCommunicator.GyroY, serialCommunicator.GyroZ);
        // ignore y and z for now
        /*
        calculatedRotation.z = serialCommunicator.GyroY;
        calculatedRotation.z = -calculatedRotation.x;
        calculatedRotation.y = -90;
        */

        

        //calculatedRotation.z = serialCommunicator.GyroY;
        calculatedRotation.z = -calculatedRotation.x;
        calculatedRotation.y = 0;
        if (useClamp)
        {
            //calculatedRotation.x = Mathf.Clamp(calculatedRotation.x, xMin, xMax);
            calculatedRotation.z = Mathf.Clamp(calculatedRotation.z, zClampMin, zClampMax);
        }
        Quaternion target = Quaternion.Euler(calculatedRotation) * Quaternion.Euler(Vector3.up * -90f);
        if (useCalculatedRotation)
             transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
            //transform.rotation = Quaternion.Euler(calculatedRotation);
        else
            transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
        
        
        //transform.Rotate(Vector3.up, -90f);
            }
}
