using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class APIRest : MonoBehaviour
{
    public InputField idInputField; // Input field for entering the project ID
    public Button submitButton;      // Button to trigger the request
    public Text helloText;           // Text to display the response
    public GameObject formPanel;     // Panel that contains the form, to be hidden on success

    private string baseUrl = "https://localhost:7044/api/funding-projects/";

    void Start()
    {
        submitButton.onClick.AddListener(OnSubmit); // Attach the submit button to the function
    }

    // Called when the submit button is pressed
    void OnSubmit()
    {
        string projectId = idInputField.text.Trim();
        if (!string.IsNullOrEmpty(projectId))
        {
            StartCoroutine(GetData(projectId));
        }
        else
        {
            Debug.LogError("Project ID is required.");
        }
    }

    IEnumerator GetData(string projectId)
    {
        // Construct the full URL with the ID as a parameter
        string url = $"{baseUrl}{projectId}";

        // Create a POST request
        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, ""))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                helloText.text = "Error: " + request.error; // Display error message
            }
            else
            {
                // Process the response data
                string json = request.downloadHandler.text;
                helloText.text = json; // Display the received data

                Debug.Log("Received data: " + json);

                // Hide the form panel if data was received successfully
                formPanel.SetActive(false);
            }
        }
    }
}