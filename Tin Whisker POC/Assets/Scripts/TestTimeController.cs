using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestTimeController : MonoBehaviour
{
    public Slider timeSlider;

    public TextMeshProUGUI timeScaleText;

    void Start()
    {
        timeSlider.value = Time.timeScale;

        timeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void OnSliderValueChanged(float value)
    {
        Time.timeScale = value;

        if (timeScaleText != null)
        {
            timeScaleText.text = "Time Scale: " + value.ToString("F2");
        }
    }
}
