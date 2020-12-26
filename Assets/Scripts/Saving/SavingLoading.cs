using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingLoading : MonoBehaviour
{
    string SavePath => $"{Application.persistentDataPath}/save.txt";
    [ContextMenu("Save")]
    void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }
    [ContextMenu("Load")]
    void Load()
    {
        var state = LoadFile();
        RestoreState(state);
    }
    private void SaveFile(object state)
    {
        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state); 
        }
    }
    void CaptureState(Dictionary<string,object> state)
    {
        foreach(var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }
    void RestoreState(Dictionary<string,object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
           if(state.TryGetValue(saveable.Id,out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
    private Dictionary<string,object> LoadFile(){
        if (!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }
        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }
}
