using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Tooltip : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipTransform;
    [SerializeField] private TextMeshProUGUI tooltipTitle;
    [SerializeField] private TextMeshProUGUI tooltipDescription;
    [SerializeField] private Vector2 offset = new Vector2(10f,10f);
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWrapSize;

    public static Tooltip instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }

        HideTooltip();
    }
    
    private void Update()
    {
        if (gameObject.activeSelf)
            UpdatePosition();
    }
    private void UpdatePosition()
    {
        Vector2 mousePos = Input.mousePosition;

        if (mousePos.x < Screen.width * 0.5f)
            tooltipTransform.pivot = new Vector2(0,1);
        else 
            tooltipTransform.pivot = new Vector2(1,1);
        tooltipTransform.position = mousePos + offset;
    }

    public void ShowTooltip(string title, string description)
    {
        gameObject.SetActive(true);
        CheckTextLength(title, description);
        tooltipTitle.text = title;
        tooltipDescription.text = description;
    }
    private void CheckTextLength(string title, string description)
    {
        if (title.Length > characterWrapSize || description.Length > characterWrapSize)
            layoutElement.enabled = true;
        else
            layoutElement.enabled = false;
    }
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }

}
