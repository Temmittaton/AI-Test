using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveSystem {
    public void NNSave(NeuralNetwork neuralNetwork){

        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create(Application.persistentDataPath + "/SaveData.dat");

        bf.Serialize(saveFile, neuralNetwork);
        saveFile.Close();
        
        Debug.Log("Data saved!");
    }

    public NeuralNetwork NNLoad(){
        string destination = Application.persistentDataPath + "/SaveData.dat";
        FileStream saveFile;

        if (File.Exists(destination)){
            saveFile = File.Open(destination, FileMode.Open);
        }
        else {
            Debug.LogError("There is no save data!");
            return new NeuralNetwork(0, 0, 0, 0);
        }

        BinaryFormatter bf = new BinaryFormatter();
        NeuralNetwork NNdata = (NeuralNetwork)bf.Deserialize(saveFile);
        saveFile.Close();

        Debug.Log("Game data loaded!");

        return NNdata;
    }

    public void ResetData(){
        string destination = Application.persistentDataPath + "/SaveData.dat";

        if (File.Exists(destination)){

            File.Delete(destination);
            Debug.Log("Data reset complete!");
        }
        else {
            Debug.LogError("No save data to delete.");
        }
    }
}