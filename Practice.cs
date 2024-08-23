using UnityEngine;
using TMPro;
using UnityEngine.UI; // Namespace for UI elements like RawImage and Image.
using System.Collections;
using UnityEngine.SceneManagement;


// Manages a practice session for learning basic hand gestures using visual, auditory, and haptic feedback.
public class Practice : MonoBehaviour
{
    // Public references to UI components and resources
    public GameObject[] keyPoints; // Detected hand keypoints.
    public RawImage[] practiceGestureImages; // Visual guides for each gesture.
    public AudioSource feedbackAudioSource; // Audio for feedback sounds.
    public AudioClip successSound; // Sound clip for successful gesture recognition.
    public Slider gestureHoldIndicator; // Indicator for gesture hold duration.
    public GameObject practiceCompletionPanel; // Panel shown upon session completion.
    public ParticleSystem signDetectedEffect; // Effect for successful gesture recognition.
    public Button nextLessonButton;

    // Gesture enumeration for session management
    private enum PracticeGesture { ThumbsUp, PeaceSign, Fist, Completed }
    private PracticeGesture currentGesture = PracticeGesture.ThumbsUp; // Current target gesture

    // Session state variables
    private float gestureTimer = 0.0f; // Timer for current gesture hold duration.
    private float gestureHoldThreshold = 5.0f; // Required hold duration for gesture recognition.
    private float peaceSignDistanceThreshold = 0.4f; // Dist. threshold for peace sign recognition.
    private float fistThreshold = 0.4f; // Dist. threshold for fist gesture recognition.

    // Called on script initialization
    void Start()
    {
        ResetPracticeSession(); // Initialize or reset practice session.
        nextLessonButton.onClick.AddListener(LoadNextLesson);
    }

    // Main update loop, called once per frame
    void Update()
    {
        if (CheckCurrentGesture())
        {
            // Correct gesture is being held
            gestureTimer += Time.deltaTime;
            gestureHoldIndicator.value = gestureTimer / gestureHoldThreshold;

            if (gestureTimer >= gestureHoldThreshold)
            {
                // Successfully held gesture for required duration
                feedbackAudioSource.PlayOneShot(successSound); // Play success sound.
                ProgressToNextGesture(); // Advance to next gesture or complete session.
            }
        }
        else
        {
            // Incorrect gesture or gesture lost
            gestureTimer = 0.0f;
            gestureHoldIndicator.value = 0;
        }
    }

    // Checks if the current gesture is correctly performed by the user.
    bool CheckCurrentGesture()
    {
        Debug.Log($"Checking gesture: {currentGesture}");
        bool result = false;

        switch (currentGesture)
        {
            case PracticeGesture.ThumbsUp:
                result = CheckForThumbsUp();
                break;
            case PracticeGesture.PeaceSign:
                result = CheckForPeaceSign();
                break;
            case PracticeGesture.Fist:
                result = CheckForFist();
                break;
            default:
                result = false;
                break;
        }

        Debug.Log($"Gesture {currentGesture} detected: {result}");
        return result;
   }

    // Detects a thumbs-up gesture based on the relative positions of fingertips.
    bool CheckForThumbsUp()
    {
        // Ensure there are enough keypoints (21 for one hand).
        if (keyPoints.Length < 21) return false;

        // Position of thumb and other fingertips.
        Vector3 thumbTip = keyPoints[4].transform.position;
        Vector3 indexTip = keyPoints[8].transform.position;
        Vector3 middleTip = keyPoints[12].transform.position;
        Vector3 ringTip = keyPoints[16].transform.position;
        Vector3 pinkyTip = keyPoints[20].transform.position;
        // ... additional fingertip positions ...

        // Thumbs-up is identified by the thumb being higher than other fingertips.
        bool thumbIsHigher = thumbTip.y > indexTip.y && thumbTip.y > middleTip.y
                            && thumbTip.y > ringTip.y && thumbTip.y > pinkyTip.y;
        return thumbIsHigher;

    }

    // Detects a peace sign gesture using positions and distances between fingertips.
    bool CheckForPeaceSign()
    {
        if (keyPoints.Length < 21) return false; // Ensure all keypoints are available.

        // Positions of index and middle fingertips.
        Vector3 indexTip = keyPoints[8].transform.position;
        Vector3 middleTip = keyPoints[12].transform.position;
        Vector3 ringTip = keyPoints[16].transform.position;
        Vector3 pinkyTip = keyPoints[20].transform.position;

         // Check if the index and middle fingers are higher than the ring and pinky fingers
        bool indexAndMiddleAreHigher = indexTip.y > ringTip.y && middleTip.y > ringTip.y && indexTip.y > pinkyTip.y && middleTip.y > pinkyTip.y;

        // Additionally, check if the index and middle fingers are not too close to each other, to ensure they are extended
        float distanceBetweenIndexAndMiddle = Vector3.Distance(indexTip, middleTip);
        bool indexAndMiddleAreSeparated = distanceBetweenIndexAndMiddle > peaceSignDistanceThreshold;
        return indexAndMiddleAreSeparated;
    }

    // Detects a fist gesture based on the proximity of all fingertips to the palm center.
    bool CheckForFist()
    {
        if (keyPoints.Length < 21) return false; // Ensure all keypoints are available.

        // Assuming keyPoints[0] is the center of the palm
        Vector3 palmCenter = keyPoints[0].transform.position;

        // Get the position of each fingertip
        Vector3 thumbTip = keyPoints[4].transform.position;
        Vector3 indexTip = keyPoints[8].transform.position;
        Vector3 middleTip = keyPoints[12].transform.position;
        Vector3 ringTip = keyPoints[16].transform.position;
        Vector3 pinkyTip = keyPoints[20].transform.position;

        // Calculate distances from the fingertips to the palm center
        float distanceThumb = Vector3.Distance(thumbTip, palmCenter);
        float distanceIndex = Vector3.Distance(indexTip, palmCenter);
        float distanceMiddle = Vector3.Distance(middleTip, palmCenter);
        float distanceRing = Vector3.Distance(ringTip, palmCenter);
        float distancePinky = Vector3.Distance(pinkyTip, palmCenter);

         // Log for debugging
         Debug.Log($"Thumb distance: {distanceThumb}, Threshold: {fistThreshold}");

        // Check if all fingertips are within the threshold distance to the palm center
        bool isFist = distanceThumb < fistThreshold && distanceIndex < fistThreshold &&
                    distanceMiddle < fistThreshold && distanceRing < fistThreshold &&
                    distancePinky < fistThreshold;
        return isFist;

    }

    // Advances the practice session to the next gesture or marks it as completed.
    void ProgressToNextGesture()
    {
        Debug.Log($"Current Gesture before progression: {currentGesture}");
        currentGesture++; // Increment the current gesture

        if (currentGesture == PracticeGesture.Completed)
        {
            HandlePracticeCompletion(); // Handle completion
        }
        else
        {
            ShowCurrentGestureImage(); // Update UI
        }

        Debug.Log($"Current Gesture after progression: {currentGesture}");
    }

    // Updates the UI to show the current gesture's visual guide.
    void ShowCurrentGestureImage()
    {
        foreach (var image in practiceGestureImages) image.enabled = false; // Disable all images.
        if (currentGesture < PracticeGesture.Completed) practiceGestureImages[(int)currentGesture].enabled = true; // Enable current gesture image.
    }

    // Handles tasks upon completing all gestures in the session.
    void HandlePracticeCompletion()
    {
        practiceCompletionPanel.SetActive(true); // Show completion panel.
        // Additional completion tasks...
    }

    // Resets the session for a new start or reinitialization.
    void ResetPracticeSession()
    {
        currentGesture = PracticeGesture.ThumbsUp; // Start from the first gesture.
        gestureTimer = 0.0f; // Reset timer.
        gestureHoldIndicator.value = 0; // Reset progress indicator.
        practiceCompletionPanel.SetActive(false); // Hide completion panel.
        ShowCurrentGestureImage(); // Display the initial gesture's guide.
    }

    public void LoadNextLesson()
    {
        
        SceneManager.LoadScene("BSLFingerSpelling"); 
    }

}
