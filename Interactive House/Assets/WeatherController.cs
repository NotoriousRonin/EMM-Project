using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeatherController : MonoBehaviour
{

    public GameObject weather;
    //Rain1,Clouds1,Fog
    ParticleSystem[] particleSystems;
    bool weatherActive=false;


    // Use this for initialization
    void Start()
    {
        particleSystems = weather.GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        checkWeatherActive();
    }

    //Schnittstelle nach außen (LeapMotion)
    public void setActivateWeather(bool b)
    {
        if (b == true)
        {
            weatherActive=true;
            manipulateEmissions(24,200,10);
        }
        else
        {
            weatherActive = false;
            manipulateEmissions(0,0,0);
        }
    }

    private void manipulateEmissions(float clouds, float rain, float fog)
    {
        var emission = particleSystems[0].emission;
        emission.rateOverTime = clouds;
        emission = particleSystems[1].emission;
        emission.rateOverTime = rain;
        emission = particleSystems[2].emission;
        emission.rateOverTime = fog;
    }

    private void checkWeatherActive()
    {
        if (weatherActive == true)
        {
            weather.SetActive(true);
        }

        if (weatherActive == false && particleSystems[0].particleCount==0)
        {
            weather.SetActive(false);
        }

    }

    

}
