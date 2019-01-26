using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class SerialCommunicator : MonoBehaviour
{
    public Vector3 initGyroRotation = new Vector3(356, 352, 201); // in degree

    public bool isInDemoMode;
    public float demoTimeVal;

    public volatile int potiTuer;
    [DisplayWithoutEdit]
    public volatile int potiTuerZu;
    [DisplayWithoutEdit]
    public volatile int potiTuerOffen;
    public volatile bool klingelState;

    /* Tuer1 */
    public volatile int sonarCM;

    public volatile bool isAlarm;
    public volatile bool magnetState;
    public volatile int lichtsensorState;
    public volatile float GyroX;
    public volatile float GyroY;
    public volatile float GyroZ;

    /* changing this will send serial commands to the arduino */
    public volatile bool isDachfensterOpen;
    private volatile bool lastIsDachfensterOpen;

    public enum DebugTestDataEnum { None, NachRechts, NachLinks, NachVorne, NachHinten };

    //This is what you need to show in the inspector.
    public DebugTestDataEnum DebugTestGyroData;

    public String COM;

    SerialPort sp = null;
    Thread thread = null;
    void Start()
    {
        // init
        potiTuerZu = 16; // zu 
        potiTuerOffen = 6; // offen

        GyroX = 356f;
        GyroY = 352f;
        GyroZ = 201f;

        isDachfensterOpen = true;
        lastIsDachfensterOpen = isDachfensterOpen;

        if (!this.enabled)
            return;

        thread = new Thread(new ThreadStart(delegate ()
        {
            Debug.Log("connecting on: " + COM);
            sp = new SerialPort(COM, 9600);

            if (!sp.IsOpen)
                sp.Open();

            while (true)
            {
                if (sp.IsOpen)
                {
                    string line = sp.ReadLine();
                    //Debug.Log(line);
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

                    // send Dachfenster state
                    if (lastIsDachfensterOpen != isDachfensterOpen)
                    {
                        Debug.Log("Sending isDachfensterOpen state to Arduino");
                        sp.Write(new[] { isDachfensterOpen ? 'u' : 'd' }, 0, 1);
                        lastIsDachfensterOpen = isDachfensterOpen;
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
            /*
            potiTuer = (int)Mathf.Lerp(potiTuerMin, potiTuerMax, demoTimeVal);

            demoTimeVal += 0.2f * Time.deltaTime;

            if (demoTimeVal > 1.0f)
            {
                int temp = potiTuerMax;
                potiTuerMax = potiTuerMin;
                potiTuerMin = temp;
                demoTimeVal = 0.0f;
            }
            */

            if(demoTimeVal < 0.5f)
            {
                sonarCM = 6; // geschlossen
            }
            else
            {
                sonarCM = 10;
            }

            switch (DebugTestGyroData)
            {
                case DebugTestDataEnum.NachRechts:
                    GyroX = 317;
                    GyroY = 352;
                    GyroZ = 261;
                    break;
                case DebugTestDataEnum.NachLinks:
                    GyroX = 31;
                    GyroY = 352;
                    GyroZ = 101;
                    break;
                case DebugTestDataEnum.NachVorne:
                    GyroX = 352;
                    GyroY = 295;
                    GyroZ = 183;
                    break;
                case DebugTestDataEnum.NachHinten:
                    GyroX = 355;
                    GyroY = 40;
                    GyroZ = 355;
                    break;
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
