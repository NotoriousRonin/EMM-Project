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

    /// <summary>
    /// Finger Positions to check for any Motion (Rain Animation)
    /// </summary>
    private float positionThumb;
    private float positionIndex;
    private float positionMiddle;
    private float positionRing;
    private float positionPinky;

    /// <summary>
    /// A flag to have the Check for Motion happen 1 Frame later
    /// </summary>
    private bool frameFlag;

    /// <summary>
    /// A flag to start and stop RainAnimation
    /// </summary>
    private bool animationFlag;
    /// <summary>
    /// Variables to check active and to be active Camera
    /// </summary>
    private int currentCamera = 1;

    /// <summary>
    /// Leap Provider Object
    /// </summary>
    private LeapProvider leapProvider;

    /// <summary>
    /// WeatherController Object
    /// </summary>
    private WeatherController weatherController;

	/// <summary>
    /// Initialize the Leap Provider Object
    /// </summary>
	void Start () {
        leapProvider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
        weatherController = GetComponent<WeatherController>();
        frameFlag = true;
        animationFlag = false;
    }
	
	/// <summary>
    /// Checks for Motion done and Interacts with Logical Object
    /// </summary>
	void Update ()
    {
        Frame frame = leapProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            //Left Hand controlls Rain Animation
            if (hand.IsLeft)
            {
                List<Finger> fingerlist = hand.Fingers;
                if (frameFlag)
                {
                    //Save all current Fingerpositions                   
                    positionThumb = fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_THUMB).TipPosition.z;
                    positionIndex = fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_INDEX).TipPosition.z;
                    positionMiddle = fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_MIDDLE).TipPosition.z;
                    positionRing = fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_RING).TipPosition.z;
                    positionPinky = fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_PINKY).TipPosition.z;
                }
                else
                {
                    //Check Tolerance Level as it can differ from Person to Person
                    //Debug.Log(Mathf.Abs(positionThumb - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_THUMB).TipPosition.z));
                    //Debug.Log(Mathf.Abs(positionIndex - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_INDEX).TipPosition.z));
                    //Debug.Log(Mathf.Abs(positionMiddle - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_MIDDLE).TipPosition.z));
                    //Debug.Log(Mathf.Abs(positionRing - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_RING).TipPosition.z));
                    //Debug.Log(Mathf.Abs(positionPinky - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_PINKY).TipPosition.z));

                    //Check if all Fingers are pointing down
                    if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_THUMB).TipPosition.y < 0.03) animationFlag = false;
                    else if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_INDEX).TipPosition.y < 0.03) animationFlag = false;
                    else if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_MIDDLE).TipPosition.y < 0.03) animationFlag = false;
                    else if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_RING).TipPosition.y < 0.03) animationFlag = false;
                    else if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_PINKY).TipPosition.y < 0.03) animationFlag = false;
                    //Check if all Fingerpositions changed 
                    else if (Mathf.Abs(positionThumb - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_THUMB).TipPosition.z) < 0.0009) animationFlag = false;
                    else if (Mathf.Abs(positionIndex - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_INDEX).TipPosition.z) < 0.0009) animationFlag = false;
                    else if (Mathf.Abs(positionMiddle - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_MIDDLE).TipPosition.z) < 0.0005) animationFlag = false;
                    else if (Mathf.Abs(positionRing - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_RING).TipPosition.z) < 0.0009) animationFlag = false;
                    else if (Mathf.Abs(positionPinky - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_PINKY).TipPosition.z) < 0.0009) animationFlag = false;               
                    else animationFlag = true;
                }

                //WeatherController depending on animationFlag
                weatherController.setActivateWeather(animationFlag);

                //Toggle frameFlag (above: else is run the frame after if)
                frameFlag = !frameFlag;              
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