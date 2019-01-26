using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kaminlicht : MonoBehaviour
{
    public Light kaminfeuer;
    int nextUpdate = 1;
    int duration = 0;
    public ParticleSystem feuer;
    public GameObject magnet;
    // Start is called before the first frame update
    void Start()
    {
        kaminfeuer.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= nextUpdate)
        {

            nextUpdate = Mathf.FloorToInt(Time.time) + 1;


            duration++;

            if (duration > 5) {
                float intensityNow = Random.Range(0.5f, 0.8f);
                kaminfeuer.intensity = intensityNow;
            }

            if (feuer.particleCount <= 10)
            {
                kaminfeuer.intensity = 0;
        
            }

        }


    }
}
