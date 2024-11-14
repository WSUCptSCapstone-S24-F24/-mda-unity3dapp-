using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightController : MonoBehaviour
{
    public Light sceneLight;             // Reference to the directional light
    public Slider brightnessSlider;      // Slider to adjust brightness
    public Slider angleSlider;           // Slider to control rotation angle
    public Button toggleButton;          // Button to toggle light on/off

    private bool isLightOn = true;       // Tracks if the light is on or off

    void Start()
    {
        // Set initial brightness and angle slider values
        if (sceneLight != null)
        {
            brightnessSlider.value = sceneLight.intensity;
            angleSlider.minValue = 0;
            angleSlider.maxValue = 360;
        }

        // Add listeners to UI controls
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        angleSlider.onValueChanged.AddListener(SetRotationAngle);
        toggleButton.onClick.AddListener(ToggleLight);

        // Initialize light rotation
        SetRotationAngle(angleSlider.value);
    }

    // Set the light’s brightness
    public void SetBrightness(float brightness)
    {
        if (sceneLight != null)
        {
            sceneLight.intensity = brightness;
        }
    }

    // Rotate the light around the origin (0,0,0) in a circular pattern
    public void SetRotationAngle(float angle)
    {
        if (sceneLight != null)
        {
            // Convert angle to radians and calculate the x and z rotation around the origin
            float radianAngle = Mathf.Deg2Rad * angle;
            Vector3 rotation = new Vector3(Mathf.Cos(radianAngle), -1f, Mathf.Sin(radianAngle));

            // Make the light look down at the origin while rotating in the x-z plane
            sceneLight.transform.rotation = Quaternion.LookRotation(rotation);
        }
    }

    // Toggle light on/off
    public void ToggleLight()
    {
        if (sceneLight != null)
        {
            isLightOn = !isLightOn;
            sceneLight.enabled = isLightOn;
        }
    }
}




