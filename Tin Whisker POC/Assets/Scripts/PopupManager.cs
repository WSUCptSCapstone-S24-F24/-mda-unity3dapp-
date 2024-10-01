using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPrefab; // Assign this prefab in the inspector
    public float displayTime = 2.0f; // Time to display each message
    private GameObject currentPopup;
    private Queue<string> messageQueue = new Queue<string>();
    private bool isDisplaying = false;

    void Update()
    {
        // If there are messages in the queue and none is currently being displayed
        if (messageQueue.Count > 0 && !isDisplaying)
        {
            StartCoroutine(DisplayMessages());
        }
    }

    public void ShowPopup(string message)
    {
        messageQueue.Enqueue(message);

        if (!isDisplaying)
        {
            StartCoroutine(DisplayMessages());
        }
    }

    private IEnumerator DisplayMessages()
    {
        while (messageQueue.Count > 0)
        {
            isDisplaying = true;
            string messageToShow = messageQueue.Dequeue();

            Debug.Log("Displaying message: " + messageToShow); // For debugging

            if (currentPopup != null)
            {
                Destroy(currentPopup);
            }

            currentPopup = Instantiate(popupPrefab, transform);
            TMP_Text popupText = currentPopup.GetComponentInChildren<TMP_Text>();
            popupText.text = messageToShow;

            CanvasGroup canvasGroup = currentPopup.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            yield return new WaitForSeconds(displayTime);

            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            Destroy(currentPopup);
        }

        isDisplaying = false;
    }

    public void HidePopup()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }
    }
}
