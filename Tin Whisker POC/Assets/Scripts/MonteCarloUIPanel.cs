using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.IO;

public class MonteCarloUIPanel : MonoBehaviour
{
    public GameObject monte_carlo_section;
    private bool hidden = true;
    // Start is called before the first frame update
    void Start()
    {
        monte_carlo_section.SetActive(false);
    }

    public void OpenBoxesOnClick()
    {
        if (hidden)
        {
            monte_carlo_section.SetActive(true);
            hidden = false;
        }
        else
        {
            monte_carlo_section.SetActive(false);
            hidden = true;
        }
    }
}
