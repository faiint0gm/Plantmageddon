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

}
