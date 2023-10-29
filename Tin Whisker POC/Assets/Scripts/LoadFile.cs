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




public class LoadFile : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public GameObject Modle; //Load OBJ Model
    public GameObject SceneManager;

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
        string[] MTLPath = StandaloneFileBrowser.OpenFilePanel("Open File", "", "mtl", false);
        if (paths.Length > 0)
        {
            Debug.Log("Selected File: " + paths[0]);
            StartCoroutine(OutputRoutineOpen(new System.Uri(paths[0]).AbsoluteUri, new System.Uri(MTLPath[0]).AbsoluteUri ));
        }
    }
#endif
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
    private IEnumerator OutputRoutineOpen(string url, string mtl)
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
            if (Modle != null)
            {
                Destroy(Modle);
            }
            Modle = new OBJLoader().Load(textStream,MTLStream);
            Modle.transform.localScale = new Vector3(150f, 150f, 150f);
            Modle.transform.Rotate(0f, 0f, 0f, Space.Self);
            Modle.name = "MainCiruitBoard";

            // Usage:
            SetLayerRecursively(Modle, LayerMask.NameToLayer("Attachables"));


            // Add sine wave movement to the loaded model
            Modle.AddComponent<SineWaveMovement>();

            // set Gravity off for Modle rigidbody
            Modle.GetComponent<Rigidbody>().useGravity = false;

            //change rigid body collision detection to continuous
            Modle.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            //Change the behavior to extrapolate
            Modle.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
            int i = 0;
            // Iterate through all the children of the parent model
            foreach (Transform child in Modle.transform)
            {
              
                // Add a kinematic rigidbody to the child
                Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.name = "CO"+i.ToString();
                child.gameObject.tag = "Part";
                rb.isKinematic = true;
                rb.useGravity = false;  // Turn off gravity
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.interpolation = RigidbodyInterpolation.Extrapolate;

                // Add a mesh collider to the child
                MeshCollider mc = child.gameObject.AddComponent<MeshCollider>();
                mc.sharedMesh = child.GetComponent<MeshFilter>().sharedMesh;
                i++;
                child.gameObject.layer = 10;
            }
            //Destroy(Modle.GetComponent<MeshCollider>());
            // Update the SceneManager
            if (Modle)
            {
                SceneManager.GetComponent<SceneHandler>().UpdateModel(Modle);
            }
            else
            {
                Debug.Log("Not MainCiruitBoard");
            }
            SceneManager.GetComponent<SceneHandler>().filePath = url;
            //SceneManager.GetComponent<SceneHandler>().fileName = Path.GetFileName(url);
            SceneManager.GetComponent<SceneHandler>().fileOpened = true;
        }
    }

}

