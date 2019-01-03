using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestsController : MonoBehaviour {

    WeatherController weatherController;
    public GameObject house;
    public Animator animator;

    void Start () {
        weatherController = this.GetComponent<WeatherController>();
    }
	
	// Update is called once per frame
	void Update () {

        tastaturListener();

    }


    void tastaturListener()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Wetter aktiviert.");
            weatherController.setActivateWeather(true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Wetter deaktiviert.");
            weatherController.setActivateWeather(false);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("animatorStart");
            animator.SetTrigger("magnetAnim");
        }

    }
}
