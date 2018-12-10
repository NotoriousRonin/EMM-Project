using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeatherController : MonoBehaviour
{

    /*
     * 
     * Ggf noch Licht-Intensität runtersetzen bei Wetter/Regen Aktivierung 
     */

    public GameObject weather;
    private bool weatherIsVisible;

    //contains all Children (Rain1,Clouds1,Fog) could be important for manipulating weather
    ParticleSystem[] particleSystems; 


    // Use this for initialization
    void Start()
    {
        weatherIsVisible = weather.activeSelf;
        particleSystems = weather.GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
       
        particleSystems= weather.GetComponentsInChildren<ParticleSystem>();
        tastaturListener();
    }

    //just for tests
    void tastaturListener()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            /*
            Debug.Log(particleSystems[0].ToString());
            Debug.Log(particleSystems[1].ToString());
            Debug.Log(particleSystems[2].ToString());
            */
            Debug.Log("Wetter aktiviert/deaktiviert.");
            activateWeather();
        }

    }

    public void activateWeather()
    {
        weatherIsVisible = !weatherIsVisible;
        weather.SetActive(weatherIsVisible);

    }

    

}
