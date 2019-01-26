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
    /// Rather the Animation is running already
    /// </summary>
    private bool animationIsRunning;

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
    /// Contains Value given by Arduino Sensors
    /// </summary>
    private SerialCommunicator serialCommunicator;

	/// <summary>
    /// Initialize the Leap Provider Object
    /// </summary>
	void Start () {
        leapProvider = FindObjectOfType<LeapServiceProvider>() as LeapServiceProvider;
        weatherController = GetComponent<WeatherController>();
        serialCommunicator = GetComponent<SerialCommunicator>();
        frameFlag = true;
        animationFlag = false;
        animationIsRunning = false;
    }

    private int lastCameraGesture = -1;
    private int cameraGestureCnt = 0;

    private float weatherGestureIgnoreTime;
    private float weatherGestureIgnmoreTimeout = 2f; // in seconds

    private float cameraGestureIgnoreTime;
    private float cameraGestureIgnoreTimeout = 0.5f; // in seconds

	/// <summary>
    /// Checks for Motion done and Interacts with Logical Object
    /// </summary>
	void Update ()
    {
        Frame frame = leapProvider.CurrentFrame;
        foreach (Hand hand in frame.Hands)
        {
            //Left Hand controlls Rain Animation
            if (hand.IsLeft && Time.time > weatherGestureIgnoreTime)
            {
                bool isWeatherAnimation = detectWeatherGesture(hand);
                animationFlag = isWeatherAnimation;     
                Debug.Log("isWeatherAnimation " + isWeatherAnimation);
               
                //Only Activate if it isn't activated yet && animationFlag = true
                if (!animationIsRunning && animationFlag != animationIsRunning)
                {
                    Debug.Log("Activate Weather");
                    animationIsRunning = true;
                    //Activate Weather
                    weatherController.setActivateWeather(true);                  
                    //Close the Roof 
                    serialCommunicator.isDachfensterOpen = false;

                    weatherGestureIgnoreTime = Time.time + weatherGestureIgnmoreTimeout;
                }

                bool isStopWeatherAnimation = detectWeatherStopGesture(hand);
                if (animationIsRunning && isStopWeatherAnimation)
                {
                    Debug.Log("Stop Weather");
                    animationIsRunning = false;
                    //Deactivate Weather
                    weatherController.setActivateWeather(false);
                    //Open the Roof
                    serialCommunicator.isDachfensterOpen = true;

                    weatherGestureIgnoreTime = Time.time + weatherGestureIgnmoreTimeout;
                }        
            }


            //Right Hand controlls active Camera
            if (hand.IsRight && Time.time > cameraGestureIgnoreTime)
            {
                int res = detectCameraGesture(hand);
                //Debug.Log(res);
                if (res != -1 && res == lastCameraGesture)
                {
                    cameraGestureCnt++;
                }
                else
                {
                    cameraGestureCnt--;
                }
                if(cameraGestureCnt < -10)
                {
                    cameraGestureCnt = 0;
                }
                Debug.Log("cameraGestureCnt " + cameraGestureCnt);
                // if gesture has been recognized n frames, change camera
                if(cameraGestureCnt > 5)
                {
                    switchCamera(res);
                    cameraGestureCnt = 0;
                    cameraGestureIgnoreTime = Time.time + cameraGestureIgnoreTimeout;
                }

                lastCameraGesture = res;
            }
        }
	}
    
    public bool detectWeatherStopGesture(Hand hand)
    {
        // If all Fingers are extended return true
        if (motionCamera(hand.Fingers, Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE, Finger.FingerType.TYPE_PINKY, Finger.FingerType.TYPE_RING, Finger.FingerType.TYPE_THUMB))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool detectWeatherGesture(Hand hand)
    {
        bool isWeatherAnimation = false;
        List<Finger> fingerlist = hand.Fingers;
        if (!animationIsRunning)
        {
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
                if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_THUMB).TipPosition.y < 0.03) isWeatherAnimation = false;
                else if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_INDEX).TipPosition.y < 0.03) isWeatherAnimation = false;
                else if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_MIDDLE).TipPosition.y < 0.03) isWeatherAnimation = false;
                else if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_RING).TipPosition.y < 0.03) isWeatherAnimation = false;
                else if (hand.PalmPosition.y - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_PINKY).TipPosition.y < 0.03) isWeatherAnimation = false;
                //Check if all Fingerpositions changed 
                else if (Mathf.Abs(positionThumb - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_THUMB).TipPosition.z) < 0.0009) isWeatherAnimation = false;
                else if (Mathf.Abs(positionIndex - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_INDEX).TipPosition.z) < 0.0009) isWeatherAnimation = false;
                else if (Mathf.Abs(positionMiddle - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_MIDDLE).TipPosition.z) < 0.0005) isWeatherAnimation = false;
                else if (Mathf.Abs(positionRing - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_RING).TipPosition.z) < 0.0009) isWeatherAnimation = false;
                else if (Mathf.Abs(positionPinky - fingerlist.Find(x => x.Type == Finger.FingerType.TYPE_PINKY).TipPosition.z) < 0.0009) isWeatherAnimation = false;
                else isWeatherAnimation = true;
            }

            //Toggle frameFlag (above: else is run the frame after if)
            frameFlag = !frameFlag;
        }
        return isWeatherAnimation;
    }


    public int detectCameraGesture(Hand hand)
    {
        List<Finger> fingerlist = hand.Fingers;
        if (motionCamera(fingerlist, Finger.FingerType.TYPE_THUMB, Finger.FingerType.TYPE_INDEX, Finger.FingerType.TYPE_MIDDLE))
            return 3;
        else if (motionCamera(fingerlist, Finger.FingerType.TYPE_THUMB, Finger.FingerType.TYPE_INDEX))
            return 2;
        else if (motionCamera(fingerlist, Finger.FingerType.TYPE_THUMB))
            return 1;
        else
            return -1;
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