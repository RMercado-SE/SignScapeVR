using UnityEngine;
using TMPro;
using UnityEngine.UI; // Namespace for UI elements like RawImage and Image.
using System.Collections;


public class BSLFingerspell : MonoBehaviour
{
    // Public GameObject references for key points on the hand. Assign these in the Unity Inspector.
    public GameObject[] keyPoints; // Array to hold all key points for easier management.

    // RawImage references for visual feedback for each letter. Assign these in the Unity Inspector.
    public RawImage[] letterImages;

    // Feedback components for sign detection. Assign these in the Unity Inspector.
    public ParticleSystem signDetectedEffect;
    public AudioSource signDetectedSound;
    public AudioClip sound; // AudioClip for auditory feedback.
    public Slider gestureHoldIndicator; // UI component to indicate gesture hold progress.
    private bool lessonStarted = false; // Tracks if the lesson has started.
    public GameObject completionPanel; // UI element shown upon lesson completion.
    private enum LetterState { None, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, Completed }
    private LetterState currentState = LetterState.None;
    private LetterState currentDetectedLetter = LetterState.None;
    private float gestureTimer = 0.0f;
    private float gestureHoldThreshold = 3.0f; // Default time in seconds a gesture must be held to confirm.
    private float customGestureHoldThreshold = 0.0f; // Custom threshold for specific gestures that might need more time.

    private Collider[] colliders; // Array to hold colliders for each key point for efficiency.

    void Start()
    {
        // Initialize colliders for each key point and hide all letter images initially.
        colliders = new Collider[keyPoints.Length];
        for (int i = 0; i < keyPoints.Length; i++)
        {
            colliders[i] = keyPoints[i].GetComponent<Collider>();
        }
        HideAllLetters();
        completionPanel.SetActive(false);
    }

    void Update()
    {
        if (!lessonStarted && CheckForThumbsUp())
        {
            StartLesson();
        }

        if (lessonStarted)
        {
            CheckForLetter();
        }
    }

    void StartLesson()
    {
        lessonStarted = true;
        currentState = LetterState.A;
        ShowLetterImage(LetterState.A);
    }

    bool CheckForThumbsUp()
    {
        // Ensure keyPoints array has at least 21 elements
        if (keyPoints.Length < 21)
        {
            Debug.LogWarning("keyPoints array does not contain enough elements.");
            return false;
        }

        // Get the position of each fingertip
        Vector3 thumbTip = keyPoints[4].transform.position;
        Vector3 indexTip = keyPoints[8].transform.position;
        Vector3 middleTip = keyPoints[12].transform.position;
        Vector3 ringTip = keyPoints[16].transform.position;
        Vector3 pinkyTip = keyPoints[20].transform.position;

        // Check if the thumb is higher than the other fingertips (assuming y-axis is up)
        bool thumbIsHigher = thumbTip.y > indexTip.y && thumbTip.y > middleTip.y
                            && thumbTip.y > ringTip.y && thumbTip.y > pinkyTip.y;

        // A thumbs-up gesture is indicated if the thumb is higher than the other fingertips and optionally extended
        return thumbIsHigher;
    }

    void CheckForLetter()
    {
        switch (currentState)
        {
            case LetterState.A: CheckForLetterA(); break;
            case LetterState.B: CheckForLetterB(); break;
            case LetterState.C: CheckForLetterC(); break;
            case LetterState.D: CheckForLetterD(); break;
            case LetterState.E: CheckForLetterE(); break;
            case LetterState.F: CheckForLetterF(); break;
            case LetterState.G: CheckForLetterG(); break;
            case LetterState.H: CheckForLetterH(); break;
            case LetterState.I: CheckForLetterI(); break;
            case LetterState.J: CheckForLetterJ(); break;
            case LetterState.K: CheckForLetterK(); break;
            case LetterState.L: CheckForLetterL(); break;
            case LetterState.M: CheckForLetterM(); break;
            case LetterState.N: CheckForLetterN(); break;
            case LetterState.O: CheckForLetterO(); break;
            case LetterState.P: CheckForLetterP(); break;
            case LetterState.Q: CheckForLetterQ(); break;
            case LetterState.R: CheckForLetterR(); break;
            case LetterState.S: CheckForLetterS(); break;
            case LetterState.T: CheckForLetterT(); break;
            case LetterState.U: CheckForLetterU(); break;
            case LetterState.V: CheckForLetterV(); break;
            case LetterState.W: CheckForLetterW(); break;
            case LetterState.X: CheckForLetterX(); break;
            case LetterState.Y: CheckForLetterY(); break;
            case LetterState.Z: CheckForLetterZ(); break;
            case LetterState.Completed: HandleLessonCompletion(); break; // Handle completion of the lesson.
        }
    }

    void CheckForLetterA()
    {
        if (colliders[8].bounds.Intersects(colliders[25].bounds)) // Condition for detecting 'A'
        {
            Debug.Log($"letterImages[1] is null: {letterImages[1] == null}");
            UpdateGestureTimer(LetterState.B, letterImages[1]); // Proceed to letter 'B' upon successful detection
            Debug.Log("A Detected");
        }
        else if (currentDetectedLetter == LetterState.A) // If the 'A' gesture is no longer detected
        {
            // Reset the timer and current detected gesture since the 'A' gesture was lost or changed
            ResetGestureTimer();
        }
    }

    // Method to detect the BSL sign for letter 'B'.
    void CheckForLetterB()
    {
        if (colliders[8].bounds.Intersects(colliders[29].bounds)) // Condition for detecting 'B'.
        {
           UpdateGestureTimer(LetterState.C,letterImages[2] ); // Proceed to letter 'C' upon successful detection
           Debug.Log("B Detected");
        }
        else if (currentDetectedLetter == LetterState.B)
        {
            ResetGestureTimer();
        }
    }

    // Method to detect the BSL sign for letter 'C'.
    void CheckForLetterC()
    {
        Vector3 thumbTip = keyPoints[4].transform.position;
        Vector3 indexTip = keyPoints[8].transform.position;  

        float someThreshold = 1.0f;

        float distance = Vector3.Distance(thumbTip, indexTip);

        if (distance < someThreshold) // Condition for detecting 'C'.
        {
           UpdateGestureTimer(LetterState.D, letterImages[3]); // Proceed to letter 'D' upon successful detection
           Debug.Log("C Detected");
        }
        else if (currentDetectedLetter == LetterState.C)
        {
            ResetGestureTimer();
        }
    }
    
    // Methiod to detect the BSL sign for letter 'D'
    void CheckForLetterD()
    {
        Vector3 thumbTip = keyPoints[4].transform.position;
        Vector3 indexTip = keyPoints[8].transform.position;  

        float someThreshold = 1.0f;

        float distance = Vector3.Distance(thumbTip, indexTip);

        if (distance < someThreshold) // Condition for detecting 'D'.
        {
           UpdateGestureTimer(LetterState.E, letterImages[4]); // Proceed to letter 'E' upon successful detection
           Debug.Log("D Detected");
        }
        else if (currentDetectedLetter == LetterState.D)
        {
            ResetGestureTimer();
        }
       
    }

    // Method to check for the BSL sign for letter 'E'.
    void CheckForLetterE()
    {
        // Condition for detecting 'E'.
        if (colliders[8].bounds.Intersects(colliders[29].bounds)) // Condition for detecting 'E'.
        {
          UpdateGestureTimer(LetterState.F, letterImages[5]); // Proceed to letter 'F' upon successful detection
          Debug.Log("E Detected");
        }
        else if (currentDetectedLetter == LetterState.E)
        {
            ResetGestureTimer();
        }
    }

    // Method to detect the BSL sign for letter 'F'.
    void CheckForLetterF()
    {
        if (colliders[7].bounds.Intersects(colliders[28].bounds) && colliders[11].bounds.Intersects(colliders[32].bounds)) // Condition for detecting 'F'.
        {
          UpdateGestureTimer(LetterState.G, letterImages[6]); // Proceed to letter 'G' upon successful detection
          Debug.Log("F Detected");
        }
        else if (currentDetectedLetter == LetterState.F)
        {
            ResetGestureTimer();
        }
    }

    // Method to detect the BSL sign for  letter 'G'.
    void CheckForLetterG()
    {
        if(colliders[4].bounds.Intersects(colliders[3].bounds)) // Condition for detecting 'G'.
        {
           UpdateGestureTimer(LetterState.H, letterImages[7]); // Proceed to letter 'H' upon successful detection
           Debug.Log("G Detected");
        }
        else if(currentDetectedLetter == LetterState.G)
        {
            ResetGestureTimer();
        }
    }

    // Method to detect the BSL sign for letter 'H'
    void CheckForLetterH()
    {
        // Temporarily use the custom threshold for 'H'
        float previousThreshold = gestureHoldThreshold;
        gestureHoldThreshold = customGestureHoldThreshold;

        // Check conditions for 'H'
        bool conditionH = (colliders[7].bounds.Intersects(colliders[28].bounds) || colliders[8].bounds.Intersects(colliders[29].bounds)) &&
                          (colliders[11].bounds.Intersects(colliders[32].bounds) || colliders[12].bounds.Intersects(colliders[33].bounds)) &&
                          (colliders[15].bounds.Intersects(colliders[36].bounds) || colliders[16].bounds.Intersects(colliders[37].bounds));

        if (conditionH)
        {
            UpdateGestureTimer(LetterState.I, letterImages[8]); // Proceed to letter 'I' upon successful detection
            Debug.Log("H Detected");
        }
        else if (currentDetectedLetter == LetterState.H)
        {
            ResetGestureTimer();
        }

        // After checking, reset the threshold back to the default value
        gestureHoldThreshold = previousThreshold;
    }

    // Method to check for the BSL sign for letter 'I'.
    void CheckForLetterI()
    {
        
        if (colliders[8].bounds.Intersects(colliders[33].bounds)) // Condition for detecting 'I'.
        {
           UpdateGestureTimer(LetterState.J, letterImages[9]); // Proceed to letter 'J' upon successful detection
           Debug.Log("I Detected");
        }
        else if (currentDetectedLetter == LetterState.I)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'J'.
    void CheckForLetterJ()
    {
        // Temporarily use the custom threshold for 'J'
        float previousThreshold = gestureHoldThreshold;
        gestureHoldThreshold = customGestureHoldThreshold;
        
        bool conditionJ = (colliders[8].bounds.Intersects(colliders[12].bounds)); // Condition for detecting 'J'.

        if(conditionJ)
        {
            UpdateGestureTimer(LetterState.K, letterImages[10]); // Proceed to letter 'K' upon successful detection
            Debug.Log("J Detected");
        }
        else if (currentDetectedLetter == LetterState.J)
        {
            ResetGestureTimer();
        }

        // After checking, reset the threshold back to the default value
        gestureHoldThreshold = previousThreshold;
    }

    // Method to check for the BSL sign for letter 'K'.
    void CheckForLetterK()
    {
        if(colliders[7].bounds.Intersects(colliders[28].bounds)) //  Condition for detecting 'K'
        {
            UpdateGestureTimer(LetterState.L,letterImages[11]); // Proceed to letter 'L' upon successful detection
            Debug.Log("K Detected");

        }
        else if(currentDetectedLetter == LetterState.K)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'L'.
    void CheckForLetterL()
    {
        if(colliders[8].bounds.Intersects(colliders[15].bounds) || colliders[8].bounds.Intersects(colliders[9].bounds) ||
           colliders[8].bounds.Intersects(colliders[6].bounds) || colliders[8].bounds.Intersects(colliders[10].bounds) ||
           colliders[8].bounds.Intersects(colliders[13].bounds) || colliders[8].bounds.Intersects(colliders[14].bounds)) //  Condition for detecting 'L'
        {
            UpdateGestureTimer(LetterState.M,letterImages[12]); // Proceed to letter 'M' upon successful detection
            Debug.Log("L Detected");

        }
        else if(currentDetectedLetter == LetterState.L)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'M'.
    void CheckForLetterM()
    {
        if (colliders[8].bounds.Intersects(colliders[5].bounds) || colliders[8].bounds.Intersects(colliders[9].bounds) ||
           colliders[8].bounds.Intersects(colliders[6].bounds) || colliders[8].bounds.Intersects(colliders[10].bounds) ||
           colliders[8].bounds.Intersects(colliders[13].bounds) || colliders[8].bounds.Intersects(colliders[14].bounds)||
           colliders[12].bounds.Intersects(colliders[5].bounds) || colliders[12].bounds.Intersects(colliders[9].bounds) ||
           colliders[12].bounds.Intersects(colliders[6].bounds) || colliders[12].bounds.Intersects(colliders[10].bounds) ||
           colliders[12].bounds.Intersects(colliders[13].bounds) || colliders[12].bounds.Intersects(colliders[14].bounds)) // Condition for detecting 'M'.
        {
          UpdateGestureTimer(LetterState.N, letterImages[13]); // Proceed to letter 'U' upon successful detection
          Debug.Log("M Detected");
        }
        else if (currentDetectedLetter == LetterState.M)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'N'.
    void CheckForLetterN()
    {
        if (colliders[8].bounds.Intersects(colliders[5].bounds) || colliders[8].bounds.Intersects(colliders[9].bounds) ||
           colliders[8].bounds.Intersects(colliders[6].bounds) || colliders[8].bounds.Intersects(colliders[10].bounds) ||
           colliders[8].bounds.Intersects(colliders[13].bounds) || colliders[8].bounds.Intersects(colliders[14].bounds)||
           colliders[12].bounds.Intersects(colliders[5].bounds) || colliders[12].bounds.Intersects(colliders[9].bounds) ||
           colliders[12].bounds.Intersects(colliders[6].bounds) || colliders[12].bounds.Intersects(colliders[10].bounds) ||
           colliders[12].bounds.Intersects(colliders[13].bounds) || colliders[12].bounds.Intersects(colliders[14].bounds)) // Condition for detecting 'N'.
        {
          UpdateGestureTimer(LetterState.O, letterImages[14]); // Proceed to letter 'O' upon successful detection
          Debug.Log("N Detected");
        }
        else if (currentDetectedLetter == LetterState.N)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'O'.
    void CheckForLetterO()
    {
        if (colliders[8].bounds.Intersects(colliders[37].bounds)) // Condition for detecting 'O'.
        {
          UpdateGestureTimer(LetterState.P, letterImages[15]); // Proceed to letter 'P' upon successful detection
          Debug.Log("O Detected");
        }
        else if (currentDetectedLetter == LetterState.O)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'P'.
    void CheckForLetterP()
    {
        if (colliders[8].bounds.Intersects(colliders[29].bounds) || colliders[12].bounds.Intersects(colliders[29].bounds)) // Condition for detecting 'P'.
        {
          UpdateGestureTimer(LetterState.Q, letterImages[16]); // Proceed to letter 'Q' upon successful detection
          Debug.Log("P Detected");
        }
        else if (currentDetectedLetter == LetterState.P)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'Q'
    void CheckForLetterQ()
    {
        if (colliders[8].bounds.Intersects(colliders[23].bounds) || colliders[7].bounds.Intersects(colliders[23].bounds)) // Condition for detecting 'Q'.
        {
          UpdateGestureTimer(LetterState.R, letterImages[17]); // Proceed to letter 'R' upon successful detection
          Debug.Log("Q Detected");
        }
        else if (currentDetectedLetter == LetterState.Q)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'R'
    void CheckForLetterR()
    {
        if (colliders[5].bounds.Intersects(colliders[38].bounds) || colliders[6].bounds.Intersects(colliders[38].bounds)) // Condition for detecting 'R'.
        {
          UpdateGestureTimer(LetterState.S, letterImages[18]); // Proceed to letter 'S' upon successful detection
          Debug.Log("R Detected");
        }
        else if (currentDetectedLetter == LetterState.R)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'S'
    void CheckForLetterS()
    {
        if (colliders[19].bounds.Intersects(colliders[40].bounds)) // Condition for detecting 'S'.
        {
          UpdateGestureTimer(LetterState.T, letterImages[19]); // Proceed to letter 'T' upon successful detection
          Debug.Log("S Detected");
        }
        else if (currentDetectedLetter == LetterState.S)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'T'
    void CheckForLetterT()
    {
        if (colliders[8].bounds.Intersects(colliders[21].bounds)) // Condition for detecting 'T'.
        {
          UpdateGestureTimer(LetterState.U, letterImages[20]); // Proceed to letter 'U' upon successful detection
          Debug.Log("T Detected");
        }
        else if (currentDetectedLetter == LetterState.R)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'U'.
    void CheckForLetterU()
    {
        if (colliders[8].bounds.Intersects(colliders[41].bounds)) // Condition for detecting 'U'.
        {
          UpdateGestureTimer(LetterState.V, letterImages[21]); // Proceed to letter 'V' upon successful detection
          Debug.Log("U Detected");
        }
        else if (currentDetectedLetter == LetterState.U)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'V'.
    void CheckForLetterV()
    {
        if (colliders[8].bounds.Intersects(colliders[5].bounds) || colliders[8].bounds.Intersects(colliders[9].bounds) ||
           colliders[8].bounds.Intersects(colliders[6].bounds) || colliders[8].bounds.Intersects(colliders[10].bounds) ||
           colliders[8].bounds.Intersects(colliders[13].bounds) || colliders[8].bounds.Intersects(colliders[14].bounds)||
           colliders[12].bounds.Intersects(colliders[5].bounds) || colliders[12].bounds.Intersects(colliders[9].bounds) ||
           colliders[12].bounds.Intersects(colliders[6].bounds) || colliders[12].bounds.Intersects(colliders[10].bounds) ||
           colliders[12].bounds.Intersects(colliders[13].bounds) || colliders[12].bounds.Intersects(colliders[14].bounds)) // Condition for detecting 'V'.
        {
          UpdateGestureTimer(LetterState.W, letterImages[22]); // Proceed to letter 'W' upon successful detection
          Debug.Log("V Detected");
        }
        else if (currentDetectedLetter == LetterState.V)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'W'.
    void CheckForLetterW()
    {
        if (colliders[7].bounds.Intersects(colliders[28].bounds)) // Condition for detecting 'W'.
        {
          UpdateGestureTimer(LetterState.X, letterImages[23]); // Proceed to letter 'X' upon successful detection
          Debug.Log("W Detected");
        }
        else if (currentDetectedLetter == LetterState.W)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'X'.
    void CheckForLetterX()
    {
        if (colliders[7].bounds.Intersects(colliders[28].bounds)) // Condition for detecting 'X'.
        {
          UpdateGestureTimer(LetterState.Y, letterImages[24]); // Proceed to letter 'Y' upon successful detection
          Debug.Log("X Detected");
        }
        else if (currentDetectedLetter == LetterState.X)
        {
            ResetGestureTimer();
        }
    }

    // Method to check for the BSL sign for letter 'Y'.
    void CheckForLetterY()
    {
        if (colliders[8].bounds.Intersects(colliders[26].bounds)) // Condition for detecting 'Y'.
        {
          UpdateGestureTimer(LetterState.Z, letterImages[25]); // Proceed to letter 'Z' upon successful detection
          Debug.Log("Y Detected");
        }
        else if (currentDetectedLetter == LetterState.Y)
        {
            ResetGestureTimer();
        }
    }

    void CheckForLetterZ()
    {
        Vector3 thumbTip = keyPoints[4].transform.position;
        Vector3 indexTip = keyPoints[8].transform.position;
        float someThreshold = 1.0f;
        float distance = Vector3.Distance(thumbTip, indexTip);

        if (distance < someThreshold)
        {
            UpdateGestureTimer(LetterState.Completed, null); // Use UpdateGestureTimer instead
        }
        else if (currentDetectedLetter == LetterState.Z)
        {
            ResetGestureTimer();
        }
    }
    void UpdateGestureTimer(LetterState detectedLetter, RawImage nextLetterImage)
    {
        if (currentDetectedLetter != detectedLetter)
        {
            currentDetectedLetter = detectedLetter;
            gestureTimer = 0.0f;
        }

        gestureTimer += Time.deltaTime;

        if (gestureHoldIndicator != null)
        {
            gestureHoldIndicator.value = gestureTimer / gestureHoldThreshold;
        }
        else
        {
            Debug.LogError("Gesture Hold Indicator Slider is not assigned in the inspector.");
        }

        if (gestureTimer >= gestureHoldThreshold)
        {
            if(detectedLetter == LetterState.Completed)
            {
                HandleLessonCompletion(); // Handle completion here
            }
            else
            {
                DetectLetter(detectedLetter, nextLetterImage);
            }
        }
    }
    void DetectLetter(LetterState nextState, RawImage nextLetterImage)
    {
        PlayFeedback();
        HideAllLetters();
        if (nextLetterImage != null)
        {
            nextLetterImage.enabled = true;
        }
        
        // Start a coroutine to handle the delay and state transition
        StartCoroutine(HandleLetterDetected(nextState, nextLetterImage));
    }

    IEnumerator HandleLetterDetected(LetterState nextState, RawImage nextLetterImage)
    {
        // Wait for a specified delay duration before continuing
        yield return new WaitForSeconds(5.0f); // Adjust the delay duration as needed

        currentState = nextState;
        if (nextState == LetterState.Completed)
        {
            HandleLessonCompletion();
        }
        else
        {
            // Reset the gesture timer for the next letter after the delay
            ResetGestureTimer();
            // Optionally, immediately show the next letter image or handle other logic here
        }
    }

    void ResetGestureTimer()
    {
        gestureTimer = 0.0f;
        currentDetectedLetter = LetterState.None;
        gestureHoldIndicator.value = 0;
    }

    void HideAllLetters()
    {
        foreach (var image in letterImages)
        {
            image.enabled = false;
        }
    }

    void ShowLetterImage(LetterState state)
    {
        int index = (int)state - 1; // Assuming LetterState enum starts with A = 1.
        if (index >= 0 && index < letterImages.Length)
        {
            letterImages[index].enabled = true;
        }
    }

    void PlayFeedback()
    {
        if (signDetectedEffect != null && !signDetectedEffect.isPlaying)
        {
            signDetectedEffect.Play();
        }
        if (signDetectedSound != null && sound != null)
        {
            signDetectedSound.PlayOneShot(sound);
        }
    }

    void HandleLessonCompletion()
    {
        completionPanel.SetActive(true);
    }

    public void RestartLesson()
    {
        HideAllLetters();
        currentState = LetterState.A;
        lessonStarted = true;
        ShowLetterImage(LetterState.A);
        completionPanel.SetActive(false);
    }

    public void ExitApplication()
    {
        Debug.Log("Exiting Application.");

        // Stop the Python script
        PythonProcessManager.StopPythonScript();

        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}