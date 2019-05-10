using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private DataManager _instance;
    public DataManager Instance
    {
        get {
            if (_instance == null)
            {
                GameObject go = new GameObject("DataManager");
                go.AddComponent<DataManager>();
                _instance = go.GetComponent<DataManager>();
            }
            return _instance;
        }        
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public static void SaveJSONData()
    {

    }
}

    public static void LoadJSONData()
    {
        string loadPath = "./Assets/Resources/JsonData/Data.json";
        using (StreamReader reader = new StreamReader(loadPath))
        {
            string json = reader.ReadToEnd();

            //data = JsonHelper.getJsonArray<HumanUnit>(json);
        }
    }
}
