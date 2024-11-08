//MIT License
//Copyright (c) 2023 DA LAB (https://www.youtube.com/@DA-LAB)
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using SFB;
using TMPro;
using UnityEngine.Networking;
using Dummiesman; //Load OBJ Model
using SimInfo;
using Unity.VisualScripting;




public class LoadFile : MonoBehaviour
{
    public GameObject Model; //Load OBJ Model
    public GameObject MainController;

#if UNITY_WEBGL && !UNITY_EDITOR
    // WebGL
    [DllImport("__Internal")]
    private static extern void UploadFile(string gameObjectName, string methodName, string filter, bool multiple);

    public void OnClickOpen() {
        UploadFile(gameObject.name, "OnFileUpload", ".obj", false);
    }

    // Called from browser
    public void OnFileUpload(string url) {
        StartCoroutine(OutputRoutineOpen(url));
    }
#else

    // Standalone platforms & editor
    public void OnClickOpen()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "obj", false);
        Debug.Log(string.Join("Paths Returned: ", paths));
        string[] MTLPath = StandaloneFileBrowser.OpenFilePanel("Open File", "", "mtl", false);

        // Check if both paths and MTLPath arrays have at least one element
        if (paths.Length > 0 && MTLPath.Length > 0)
        {
            Debug.Log("Selected File: " + paths[0]);
            StartCoroutine(OutputRoutineOpen(new System.Uri(paths[0]).AbsoluteUri, new System.Uri(MTLPath[0]).AbsoluteUri));
        }
        else
        {
            Debug.Log("No file selected.");
        }
    }
#endif


    public void LoadFromPath(string objPath, string mtlPath)
    {
        StartCoroutine(OutputRoutineOpen(objPath, mtlPath));
    }
    
    private static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
            return;
        
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
                continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Function to change the height of the loaded object
    private void SetPosition(float xPos, float yPos, float zPos)
    {
        if (Model != null)
        {
            Vector3 newPosition = Model.transform.position;
            newPosition.x = xPos;
            newPosition.y = yPos;
            newPosition.z = zPos;
            Model.transform.position = newPosition;
        }
    }

    private IEnumerator OutputRoutineOpen(string url, string mtl = null)
    {
        Debug.Log("File URI: " + url);
        UnityWebRequest www = UnityWebRequest.Get(url);
        UnityWebRequest mmm = UnityWebRequest.Get(mtl);
        yield return www.SendWebRequest();
        yield return mmm.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("WWW ERROR: " + www.error);
        }
        else
        {
            MemoryStream textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.downloadHandler.text));
            MemoryStream MTLStream = new MemoryStream(Encoding.UTF8.GetBytes(mmm.downloadHandler.text));
            if (Model != null)
            {
                Destroy(Model);
            }
            Model = new OBJLoader().Load(textStream, MTLStream);
            if (Model == null)
            {
                // TODO: Flash message to user
                Debug.Log("Error loading OBJ model.");
                yield break; // Exit the coroutine early if loading OBJ model fails
            }
            SetPosition(0, 0, 0);
            Model.name = "MainCiruitBoard";

            
            // Usage:
            SetLayerRecursively(Model, LayerMask.NameToLayer("Attachables"));

            Model.tag = "Board";

            // Add sine wave movement to the loaded model
            Model.AddComponent<SineWaveMovement>();

            // set Gravity off for Model rigidbody
            Model.GetComponent<Rigidbody>().useGravity = false;

            //change rigid body collision detection to continuous
            Model.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            //Change the behavior to extrapolate
            Model.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
            int i = 0;
            // Iterate through all the children of the parent model
            foreach (Transform child in Model.transform)
            {
                // Add a kinematic rigidbody to the child
                Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.name = "CO"+i.ToString();
                if(i != 0) { child.gameObject.tag = "Part"; }
                rb.isKinematic = true;
                rb.useGravity = false;  // Turn off gravity
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.interpolation = RigidbodyInterpolation.Extrapolate;

                // Add a mesh collider to the child
                MeshCollider mc = child.gameObject.AddComponent<MeshCollider>();
                mc.sharedMesh = child.GetComponent<MeshFilter>().sharedMesh;
                i++;
                child.gameObject.layer = 6;
            }

            // Save the file path to the scene handler, to be used in the Monte Carlo simulation
            MainController.GetComponent<MainController>().objfilePath = url;
            MainController.GetComponent<MainController>().mtlfilePath = mtl;
            
            MainController.GetComponent<MainController>().PCBloaded = true;
            MainController.GetComponent<MainController>().ui_unlock();
        }
    }

}

