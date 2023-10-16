using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocker : MonoBehaviour
{
    public GameObject button1;
    public GameObject textBox1;
    public GameObject textBox2;
    private bool hidden = true;

    // Start is called before the first frame update
    void Start()
    {
        button1.SetActive(false);
        textBox1.SetActive(false);
        textBox2.SetActive(false);
    }

    public void OpenTextboxesOnClick()
    {
        if (hidden)
        {
            button1.SetActive(true);
            textBox1.SetActive(true);
            textBox2.SetActive(true);
            hidden = false;
        }
        else
        {
            button1.SetActive(false);
            textBox1.SetActive(false);
            textBox2.SetActive(false);
            hidden = true;
        }
    }
}
