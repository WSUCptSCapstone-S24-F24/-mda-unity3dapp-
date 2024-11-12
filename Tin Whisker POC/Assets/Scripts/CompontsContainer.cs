using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class ComponentsContainer
{
    private static Dictionary<string, List<GameObject>> MaterialsComponents = new Dictionary<string, List<GameObject>>();

    public static void AddComponent(string materialName, GameObject component)
    {
        if (!MaterialsComponents.ContainsKey(materialName))
        {
            MaterialsComponents[materialName] = new List<GameObject>();
        }
        MaterialsComponents[materialName].Add(component);
    }

    public static void RemoveComponent(string materialName, GameObject component)
    {
        if (MaterialsComponents.ContainsKey(materialName))
        {
            MaterialsComponents[materialName].Remove(component);
            if (MaterialsComponents[materialName].Count == 0)
            {
                MaterialsComponents.Remove(materialName); // Clean up empty keys
            }
        }
    }

    public static List<GameObject> GetComponentsByMaterial(string materialName)
    {
        if (MaterialsComponents.TryGetValue(materialName, out var components))
        {
            return components;
        }
        return null;
    }

    public static List<GameObject> GetAllComponents()
    {
        return MaterialsComponents.Values.SelectMany(components => components).ToList();
    }

    public static List<string> GetAllMaterials()
    {
        return new List<string>(MaterialsComponents.Keys);
    }

    public static void ClearAllComponents()
    {
        MaterialsComponents.Clear();
    }
}
