using UnityEngine;
using UnityEngine.UI;

public class ResetTextBoxPos : MonoBehaviour
{
    public GameObject textBox; // Reference to the parent container GameObject

    // Call this method when you want to reset the top position of the text box
    public void ResetTopPosition()
    {
        // Force a layout update to ensure ContentSizeFitter has updated the size
        LayoutRebuilder.ForceRebuildLayoutImmediate(textBox.GetComponent<RectTransform>());

        // Get the RectTransform component of the parent container
        RectTransform textBoxRectTransform = textBox.GetComponent<RectTransform>();

        // Calculate the desired position based on the size of the text content
        float desiredXPosition = CalculateDesiredXPosition(textBoxRectTransform);
        float desiredYPosition = CalculateDesiredYPosition(textBoxRectTransform);

        // Update the position of the parent container to shift it downward
        textBoxRectTransform.anchoredPosition = new Vector2(desiredXPosition, desiredYPosition);
    }

    // This method calculates the desired Y position based on the size of the text content
    private float CalculateDesiredYPosition(RectTransform textBoxRectTransform)
    {
        // Calculate the half height of the text box
        float halfHeight = textBoxRectTransform.rect.height / 2f;

        // Calculate the desired Y position by adding half of the text box's height to its current position
        float desiredYPosition = textBoxRectTransform.anchoredPosition.y - halfHeight;

        return desiredYPosition;
    }

    // This method calculates the desired X position based on the size of the text content
    private float CalculateDesiredXPosition(RectTransform textBoxRectTransform)
    {
        // Calculate the half width of the text box
        float halfWidth = textBoxRectTransform.rect.width / 2f;

        // Calculate the desired X position by adding half of the text box's width to its current position
        float desiredXPosition = textBoxRectTransform.anchoredPosition.x + halfWidth;

        return desiredXPosition;
    }
}
