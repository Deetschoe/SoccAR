using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GetRequestOnKeyPress : MonoBehaviour
{
    public GameObject targetGameObject; // The game object to check proximity with
    public float proximityThreshold = 5.0f; // Distance threshold for proximity check
    public GameObject rightFoot; // The right foot game object

    private bool wasWithinProximity = false; // Tracks if the right foot was within proximity in the previous frame

    void Start()
    {
        if (targetGameObject == null)
        {
            Debug.LogError("Target GameObject is not set.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TurnOnElectric();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TurnOffElectric();
        }

        if (rightFoot != null && targetGameObject != null)
        {
            float distance = Vector3.Distance(rightFoot.transform.position, targetGameObject.transform.position);
            bool isWithinProximity = distance <= proximityThreshold;

            // Check for change in proximity state
            if (isWithinProximity && !wasWithinProximity)
            {
                // Entered proximity
                TurnOnElectric();
                wasWithinProximity = true;
            }
            else if (!isWithinProximity && wasWithinProximity)
            {
                // Exited proximity
                TurnOffElectric();
                wasWithinProximity = false;
            }
        }
    }

    void TurnOnElectric()
    {
        // To prevent spamming the network request, ensure we only call it due to a proximity change
        if (!wasWithinProximity)
        {
            StartCoroutine(SendGetRequestCoroutine("https://unbiased-llama-initially.ngrok-free.app/send-command/0"));
        }
    }

    void TurnOffElectric()
    {
        // To prevent spamming the network request, ensure we only call it due to a proximity change
        if (wasWithinProximity)
        {
            StartCoroutine(SendGetRequestCoroutine("https://unbiased-llama-initially.ngrok-free.app/send-command/1"));
        }
    }

    IEnumerator SendGetRequestCoroutine(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                Debug.Log("Received response: " + responseText);
            }
        }
    }
}
