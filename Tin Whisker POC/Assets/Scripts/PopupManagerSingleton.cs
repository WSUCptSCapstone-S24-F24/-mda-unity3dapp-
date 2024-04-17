using UnityEngine;

public class PopupManagerSingleton : MonoBehaviour
{
    // Static reference to the instance
    private static PopupManagerSingleton _instance;
    public static PopupManagerSingleton Instance => _instance;

    // Reference to the PopupManager component
    public PopupManager popupManager;

    private void Awake()
    {
        // Ensure only one instance of PopupManagerSingleton exists
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Method to show popup messages
    public void ShowPopup(string message)
    {
        popupManager?.ShowPopup(message);
    }
}
