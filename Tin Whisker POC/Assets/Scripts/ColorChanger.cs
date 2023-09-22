using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public Color targetColor;


    private MeshRenderer objectRenderer;
    private bool hasCollidedWithObject1;
    private bool hasCollidedWithObject2;

    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>();
        hasCollidedWithObject1 = false;
        hasCollidedWithObject2 = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject == object1)
        {
            //Renderer renderer = object1.GetComponent<Renderer>();
            //Material uniqueMaterial = renderer.material;
           // if (uniqueMaterial.getColor() == "gray")
            //{
                hasCollidedWithObject1 = true;
            //}
           
        }

        if (collision.gameObject == object2)
        {
            // Renderer renderer = object1.GetComponent<Renderer>();
            // Material uniqueMaterial = renderer.material;
            // if (uniqueMaterial.getColor() == "gray")
            // {
            hasCollidedWithObject2 = true;
            //}
        }

        if (hasCollidedWithObject1 && hasCollidedWithObject2)
        {
            objectRenderer.material.color = targetColor;
        }
    }

}
