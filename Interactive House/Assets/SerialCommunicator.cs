using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class SerialCommunicator : MonoBehaviour
{
    public bool isInDemoMode;
    public float demoTimeVal;

    public volatile int potiTuer;
    [DisplayWithoutEdit]
    public volatile int potiTuerMin;
    [DisplayWithoutEdit]
    public volatile int potiTuerMax;
    public volatile bool klingelState;

    /* Tuer1 */
    public volatile int sonarCM;

    public volatile bool isAlarm;
    public volatile bool magnetState;
    public volatile int lichtsensorState;
    public volatile float GyroX;
    public volatile float GyroY;
    public volatile float GyroZ;

    SerialPort sp = null;
    Thread thread = null;
    void Start()
    {
        // init
        potiTuerMin = 12;
        potiTuerMax = 16;


        if (!this.enabled)
            return;

        thread = new Thread(new ThreadStart(delegate ()
        {
            sp = new SerialPort("COM7", 9600);

            if (!sp.IsOpen)
                sp.Open();

            while (true)
            {
                if (sp.IsOpen)
                {
                    string line = sp.ReadLine();
                    try
                    {
                    /*
                     * Serial.println(String(potiTuer) + "," + String(klingelState) + "," + String(sonarCM) + "," + String(isAlarm) + ","   
                        + String(magnetState) + "," 
                        + String(lichtsensorState) + ","
                        // Gyro x, y, z
                                            + String(x) + ","
                                            + String(y) + ","
                                            + String(z)
                            );
                    */ 
                         
                        string[] lineSplit = line.Split(',');
                        potiTuer = Convert.ToInt32(lineSplit[0]);
                        klingelState = lineSplit[1] == "1";
                        sonarCM = Convert.ToInt32(lineSplit[2]);
                        isAlarm = lineSplit[3] == "1";
                        magnetState = lineSplit[4] == "1";
                        lichtsensorState = Convert.ToInt32(lineSplit[5]);
                        GyroX = float.Parse(lineSplit[6]);
                        GyroY = float.Parse(lineSplit[7]);
                        GyroZ = float.Parse(lineSplit[8]);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("could not parse " + ex.Message);
                    }
                }
                else
                {
                    Debug.Log("Com not open");
                }
            }
        }));

        // only start if is not in demo mode
        if(isInDemoMode == false)
            thread.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInDemoMode)
        {
            potiTuer = (int)Mathf.Lerp(potiTuerMin, potiTuerMax, demoTimeVal);

            demoTimeVal += 0.2f * Time.deltaTime;

            if (demoTimeVal > 1.0f)
            {
                int temp = potiTuerMax;
                potiTuerMax = potiTuerMin;
                potiTuerMin = temp;
                demoTimeVal = 0.0f;
            }

            if(demoTimeVal < 0.5f)
            {
                sonarCM = 6; // geschlossen
            }
            else
            {
                sonarCM = 10;
            }

        }
    }

    void OnApplicationQuit()
    {
        if (thread != null)
            thread.Abort();

        if (sp != null && sp.IsOpen)
            sp.Close();
    }
}
