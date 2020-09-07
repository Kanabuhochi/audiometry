using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
 using System.Runtime.Serialization.Formatters.Binary;
 
 public class loadData : MonoBehaviour { 

	public Text text;

    void Start()
    {
        LoadFile();
    }
 
     public void LoadFile()
     {
             string filePath = Application.persistentDataPath + "/ConfigData.json";

			 string jsonString = File.ReadAllText(filePath);

			 SaveData configs = JsonUtility.FromJson<SaveData>(jsonString);
  
 
        foreach (Config config in configs.config)
        {
            text.text = config.name + " - Impedancja: " + config.impendation + " Skuteczność: " + config.power;
        }
	 }
 }