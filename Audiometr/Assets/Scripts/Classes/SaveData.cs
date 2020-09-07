using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
 {  
     public List<Config> config = new List<Config>();
     public SaveData(string nameStr, string imp, string pow )
     {
        Config con = new Config();
        con.name = nameStr;
        con.impendation = imp;
        con.power = pow;
        config.Add(con);
    }
 }


[System.Serializable]
public class Config{
    public string name;
    public string impendation;
    public string power;
}