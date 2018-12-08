using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LeapController : MonoBehaviour {
    
    /// <summary>
    /// Camera Objects to switch between
    /// </summary>
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;

    //public Camera[] cameras;

    /// <summary>
    /// Variables to check active and to be active Camera
    /// </summary>
    private int currentCamera = 1;

    /// <summary>
    /// Leap Provider Object
    /// </summary>
    private LeapProvider leapProvider;

	/// <summary>
    /// Initialize the Leap Provider Object
    /// </summary>
	void Start () {
        leapProvider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
    }
	
	/// <summary>
    /// Checks for Motion done and Interacts with Logical Object
    /// </summary>
	void Update () {
        Frame frame = leapProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            //Left Hand controlls Rain Animation
            if (hand.IsLeft)
            {
                /*
                if (motionRain(hand)) //StartAnimation;
                else //StopAnimation;
                */
            }
            //Right Hand controlls active Camera
            if (hand.IsRight)
            {
                List<Finger> fingerlist = hand.Fingers;
                if (motionCamera(fingerlist, Finger.FingerType.TYPE_THUMB, Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE))
                    switchCamera(3);
                else if (motionCamera(fingerlist, Finger.FingerType.TYPE_THUMB, Finger.FingerType.TYPE_INDEX))
                    switchCamera(2);
                else if (motionCamera(fingerlist, Finger.FingerType.TYPE_THUMB))
                    switchCamera(1);                
            }
        }
	}

    /// <summary>
    /// Checks if the Motion for Rain was done
    /// All Fingers pointing downwards and moving
    /// </summary>
    /// <param name="hand"> The Hand the Motion should be done with</param>
    /// <returns>TRUE if Motion was done else FALSE</returns>
    private bool motionRain(Hand hand)
    {
        return false;
    }

    /// <summary>
    /// Checks if the FingerTypes given are extended and the others not
    /// </summary>
    /// <param name="fingerlist">Fingers of the Hand the Motion is done with</param>
    /// <param name="fingerTypes">The only Fingers that should be extended</param>
    /// <returns>TRUE if given FingerTypes are the only extended Fingers</returns>
    private bool motionCamera(List<Finger> fingerlist, params Finger.FingerType[] fingerTypes)
    {
        if (fingerlist.FindAll(x => x.IsExtended).Count != (fingerTypes.Length)) return false;
        foreach (Finger.FingerType type in fingerTypes)
        {
            if (fingerlist.Find(x => x.Type == type).IsExtended == false) return false ; 
        }       
        return true;
    }


    /// <summary>
    /// Switch Camera
    /// <param name="newCamera">#Camera to switch to</param>
    /// </summary>
    private void switchCamera(int newCamera)
    {       
        switch (currentCamera)
        {
            case 1:
                camera1.gameObject.SetActive(false);
                break;
            case 2:
                camera2.gameObject.SetActive(false);
                break;
            case 3:
                camera3.gameObject.SetActive(false);
                break;
        }

        switch (newCamera)
        {
            case 1:
                camera1.gameObject.SetActive(true);
                break;
            case 2:
                camera2.gameObject.SetActive(true);
                break;
            case 3:
                camera3.gameObject.SetActive(true);
                break;
        }

        currentCamera = newCamera;
    }
}
