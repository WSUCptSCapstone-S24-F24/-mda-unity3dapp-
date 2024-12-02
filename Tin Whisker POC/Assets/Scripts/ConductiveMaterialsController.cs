using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConductiveMaterialsController : MonoBehaviour
{
    private int boardNum = -1;
    private static Dictionary<string, bool> conductiveMaterials = new Dictionary<string, bool>();
    private static Dictionary<string, Color> matColors = new Dictionary<string, Color>();

    [SerializeField] private Transform contentParent; // Assign the "Content" GameObject from the Scroll View hierarchy
    [SerializeField] private GameObject toggleTemplate; // Assign the disabled Toggle Template GameObject

    public void OnEnable()
    {
        int currentBoardNum = LoadFile.LoadNumber;
        if (currentBoardNum != boardNum)
        {
            boardNum = currentBoardNum;
            conductiveMaterials.Clear();
            matColors.Clear();

            foreach (Transform child in contentParent)
            {
                if (child.gameObject != toggleTemplate)
                {
                    Destroy(child.gameObject);
                }
            }

            List<string> allMaterials = ComponentsContainer.GetAllMaterials();
            foreach (var mat in allMaterials)
            {
                AddMaterial(mat);
            }
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
            HandleConductiveChange(material, isOn);
        });
    }

    public void HandleConductiveChange(string material, bool isOn)
    {
        if (!conductiveMaterials.ContainsKey(material))
        {
            Debug.LogError($"Material {material} does not exist.");
            return;
        }

        List<GameObject> materialComponents = ComponentsContainer.GetComponentsByMaterial(material);
        foreach (var comp in materialComponents)
        {
            var renderer = comp.GetComponent<Renderer>();
            if (renderer != null)
            {
                if (!matColors.ContainsKey(material))
                {
                    matColors[material] = renderer.material.color;
                }
                if (isOn)
                {
                    renderer.material.color = Color.red;
                    conductiveMaterials[material] = true;
                    comp.tag = "Conductive";
                }
                else
                {
                    renderer.material.color = matColors[material];
                    conductiveMaterials[material] = false;
                    comp.tag = "Part";
                }
            }
        }
    }
}
