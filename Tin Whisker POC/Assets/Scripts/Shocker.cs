using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocker : MonoBehaviour
{
    public GameObject textBox1;
    public GameObject textBox2;
    private bool hidden = true;
    public bool shocking = false;

    // Start is called before the first frame update
    void Start()
    {
        textBox1.SetActive(false);
        textBox2.SetActive(false);
        shocking = false;
    }

    public void OpenTextboxesOnClick()
    {
        if (hidden)
        {
            textBox1.SetActive(true);
            textBox2.SetActive(true);
            hidden = false;
            shocking = true;
        }
        else
        {
            textBox1.SetActive(false);
            textBox2.SetActive(false);
            hidden = true;
            shocking = false;
        }
    }
}
