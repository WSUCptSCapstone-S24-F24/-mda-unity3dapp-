using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenVibration : MonoBehaviour
{
    public GameObject textBox1;
    public GameObject textBox2;
    private bool hidden = true;
    public bool vibrate = false;

    // Start is called before the first frame update
    void Start()
    {
        textBox1.SetActive(false);
        textBox2.SetActive(false);
        vibrate = false;
    }

    public void OpenTextboxesOnClick()
    {
        if(hidden)
        {
            textBox1.SetActive(true);
            textBox2.SetActive(true);
            hidden = false;
            vibrate = true;
        }
        else
        {
            textBox1.SetActive(false);
            textBox2.SetActive(false);
            hidden = true;
            vibrate = false;
        }
    }
}
