using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get { return instance; } }
    private static SaveManager instance;

    //Fields
    public SaveState save;
    private const string saveFileName = "SaveState.ss";
    private BinaryFormatter formatter;

    //Actions

    public Action<SaveState> OnLoad;
    public Action<SaveState> OnSave;

    private void Awake()
    {
        formatter = new BinaryFormatter();
        Load();
    }

    public void Load()
    {
        try
        {
            FileStream file = new FileStream(saveFileName, FileMode.Open, FileAccess.Read);
            save = formatter.Deserialize(file) as SaveState;//deserialize
            file.Close();
            OnLoad?.Invoke(save);
        }
        catch
        {
            Debug.Log("Save file not found - create a new file");
            Save();
        }
    }

    public void Save()
    {
        //If there is no save state foind, create a new one
        if (save == null)
            save = new SaveState();

        //Set the time that we tried to save
        save.LastSaveTime = DateTime.Now; 
        
        //Open a file on our system & write to it
        FileStream file = new FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.Write);
        formatter.Serialize(file, save);
        file.Close();

        OnSave?.Invoke(save);

    }
}
