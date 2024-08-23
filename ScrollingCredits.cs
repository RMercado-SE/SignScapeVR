using UnityEngine;
using UnityEngine.SceneManagement; // Needed for changing scenes
using System.Diagnostics; // Needed for Process

public class ScrollingCredits : MonoBehaviour
{
    public float scrollSpeed = 10f;
    public Transform WelcomeText; // Reference to the WelcomeText
    public Transform Button; // Reference to the Button

    void Start()
    {
        PythonProcessManager.StartPythonScript();
    }

    void Update()
    {
        if (Button.position.y < 500)
        {
            WelcomeText.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            Button.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
        }
    }

    // Method to be called when the button is clicked
    public void LoadNextScene()
    {
        SceneManager.LoadScene("PracticeScene");
    }
}
