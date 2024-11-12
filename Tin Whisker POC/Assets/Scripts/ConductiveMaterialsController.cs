using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConductiveMaterialsController : MonoBehaviour
{
    private Dictionary<string, bool> conductiveMaterials = new Dictionary<string, bool>();
    private Dictionary<string, Color> matColors = new Dictionary<string, Color>();

    [SerializeField] private Transform contentParent; // Assign the "Content" GameObject from the Scroll View hierarchy
    [SerializeField] private GameObject toggleTemplate; // Assign the disabled Toggle Template GameObject

    public void OnEnable()
    {
        conductiveMaterials.Clear();
        matColors.Clear();

        List<string> allMaterials = ComponentsContainer.GetAllMaterials();
        foreach (var mat in allMaterials)
        {
            AddMaterial(mat);
        }
    }

    public void AddMaterial(string material)
    {
        if (conductiveMaterials.ContainsKey(material))
        {
            Debug.LogWarning($"Material {material} already exists.");
            return;
        }

        // Add material to the dictionary
        conductiveMaterials[material] = false;

        // Initialize default color if not set
        if (!matColors.ContainsKey(material))
        {
            matColors[material] = Color.white; // Default to white
        }

        // Create a new toggle from the template
        GameObject newToggle = Instantiate(toggleTemplate, contentParent);
        newToggle.name = material + " Toggle";
        newToggle.SetActive(true);

        // Set up the toggle label and functionality
        Toggle toggleComponent = newToggle.GetComponent<Toggle>();
        Text label = newToggle.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = material;
        }

        // Add listener to update dictionary when toggle changes
        toggleComponent.onValueChanged.AddListener(isOn =>
        {
            conductiveMaterials[material] = isOn;
        });
    }

    public void SetConductive(string material)
    {
        if (!conductiveMaterials.ContainsKey(material))
        {
            Debug.LogError($"Material {material} does not exist.");
            return;
        }

        List<GameObject> materialComponents = ComponentsContainer.GetComponentsByMaterial(material);
        foreach (var comp in materialComponents)
        {
            comp.tag = "Conductive";
            var renderer = comp.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.red;
            }
        }

        // Update the dictionary
        conductiveMaterials[material] = true;

        // Find the corresponding toggle and set it to true
        Transform toggleTransform = contentParent.Find(material + " Toggle");
        if (toggleTransform != null)
        {
            Toggle toggleComponent = toggleTransform.GetComponent<Toggle>();
            toggleComponent.isOn = true;
        }
    }

    public void SetNonConductive(string material)
    {
        if (!conductiveMaterials.ContainsKey(material))
        {
            Debug.LogError($"Material {material} does not exist.");
            return;
        }

        if (!matColors.ContainsKey(material))
        {
            Debug.LogError($"Material color for {material} not found.");
            return;
        }

        List<GameObject> materialComponents = ComponentsContainer.GetComponentsByMaterial(material);
        foreach (var comp in materialComponents)
        {
            comp.tag = "Untagged";
            var renderer = comp.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = matColors[material];
            }
        }

        // Update the dictionary
        conductiveMaterials[material] = false;

        // Find the corresponding toggle and set it to false
        Transform toggleTransform = contentParent.Find(material + " Toggle");
        if (toggleTransform != null)
        {
            Toggle toggleComponent = toggleTransform.GetComponent<Toggle>();
            toggleComponent.isOn = false;
        }
    }
}
