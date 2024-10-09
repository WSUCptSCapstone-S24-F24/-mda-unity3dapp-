using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipTarget : MonoBehaviour
{
    [SerializeField] private string titleText;
    [TextArea(3, 6)]
    [SerializeField] private string descriptionText;

    public void OnShowTooltip()
    {
        Tooltip.instance.ShowTooltip(titleText, descriptionText);
    }

    public void OnHideTooltip()
    {
        Tooltip.instance.HideTooltip();
    }
}
