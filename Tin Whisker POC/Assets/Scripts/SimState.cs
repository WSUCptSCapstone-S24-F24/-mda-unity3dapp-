using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SimInfo
{
    public class SimState
    {
        public int whiskerDensity;
        public float spawnAreaSizeX;
        public float spawnAreaSizeY;
        public float spawnAreaSizeZ;
        public float spawnPositionX;
        public float spawnPositionY;
        public float spawnPositionZ;
        public float LengthMu;
        public float LengthSigma;
        public float WidthMu;
        public float WidthSigma;
        public int simNumber;
        public float simDuration;
        public string objfilePath;
        public string mtlfilePath;
        public bool fileOpened;


        public SimState(){
            // Default values
            this.whiskerDensity = 10;
            this.spawnAreaSizeX = 2f;
            this.spawnAreaSizeY = 2f;
            this.spawnAreaSizeZ = 2f;
            this.spawnPositionX = 0f;
            this.spawnPositionY = 0f;
            this.spawnPositionZ = 15f;
            this.LengthMu = 0.5f;
            this.LengthSigma = 0.5f;
            this.WidthMu = 0.5f;
            this.WidthSigma = 0.5f;
            this.simNumber = -1;
        }

        public SimState(int whiskerDensity, float spawnAreaSizeX, float spawnAreaSizeY, float spawnAreaSizeZ, 
                        float spawnPositionX, float spawnPositionY, float spawnPositionZ, float LengthMu, 
                        float LengthSigma, float WidthMu, float WidthSigma, int simNumber)
        {
            this.whiskerDensity = whiskerDensity;
            this.spawnAreaSizeX = spawnAreaSizeX;
            this.spawnAreaSizeY = spawnAreaSizeY;
            this.spawnAreaSizeZ = spawnAreaSizeZ;
            this.spawnPositionX = spawnPositionX;
            this.spawnPositionY = spawnPositionY;
            this.spawnPositionZ = spawnPositionZ;
            this.LengthMu = LengthMu;
            this.LengthSigma = LengthSigma;
            this.WidthMu = WidthMu;
            this.WidthSigma = WidthSigma;
            this.simNumber = simNumber;
        }

        public void SaveSimToJSON(string jsonPath)
        {
            Debug.Log("attempting to save sim to JSON | Path: " + jsonPath);
            // Serialize the simState to JSON
            string jsonString = JsonUtility.ToJson(this);

            Debug.Log("Saving -> JSON string:\n" + jsonString);
            // Create a file path and file name for the JSON file
            string filePath = jsonPath;

            // Write the JSON string to a file synchronously
            File.WriteAllText(filePath, jsonString);
        }

        public IEnumerator SaveSimToJSONasync(string jsonPath)
        {
            // Serialize the simState to JSON
            string jsonString = JsonUtility.ToJson(this);
            
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
