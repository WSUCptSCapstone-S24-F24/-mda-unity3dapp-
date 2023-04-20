using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SimInfo
{
    public class SimState
    {
        public int WhiskerCount;
        public float spawnAreaSize;
        public float heightAboveCircuitBoard;
        public float LengthMu;
        public float LengthSigma;
        public float WidthMu;
        public float WidthSigma;
        public int SimCount;
        public GameObject Model;

        public SimState(){
            // Default values
            this.WhiskerCount = 10;
            this.spawnAreaSize = 2f;
            this.heightAboveCircuitBoard = 10f;
            this.LengthMu = 0.5f;
            this.LengthSigma = 0.5f;
            this.WidthMu = 0.5f;
            this.WidthSigma = 0.5f;
            this.SimCount = -1;
        }

        public SimState(int WhiskerCount, float spawnAreaSize, float heightAboveCircuitBoard, float LengthMu, float LengthSigma, float WidthMu, float WidthSigma, int SimCount)
        {
            this.WhiskerCount = WhiskerCount;
            this.spawnAreaSize = spawnAreaSize;
            this.heightAboveCircuitBoard = heightAboveCircuitBoard;
            this.LengthMu = LengthMu;
            this.LengthSigma = LengthSigma;
            this.WidthMu = WidthMu;
            this.WidthSigma = WidthSigma;
            this.SimCount = SimCount;
        }

        public IEnumerator SaveSimToJSON(string jsonPath)
        {
            Debug.Log("attempting to save sim to JSON");
            // Serialize the simState to JSON
            string jsonString = JsonUtility.ToJson(this);

            Debug.Log("Saving -> JSON string:\n" + jsonString);
            // Create a file path and file name for the JSON file
            string filePath = jsonPath;

            // Write the JSON string to a file asynchronously
            var asyncRequest = File.WriteAllTextAsync(filePath, jsonString);

            // Wait for the file writing to complete
            while (!asyncRequest.IsCompleted)
            {
                yield return null;
            }
        }
    }
    
}
