using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [HideInInspector]
    public bool[] completedLevel = { false, false, false, false };

    [HideInInspector]
    public int currentCompletedLevels;
    public static ProgressManager instance;

    [HideInInspector]
    public int currentLevel;

    [HideInInspector] public bool firstTime = true;
    
    private void Awake()
    {
        if (!instance)
        {
            currentCompletedLevels = 0;
            currentLevel = 3;

            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    
    public void SaveGame()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
 
        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
 
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, currentCompletedLevels);
        file.Close();
    }
 
    public bool LoadGame()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;
 
        if(File.Exists(destination)) file = File.OpenRead(destination);
        else return false;

        BinaryFormatter bf = new BinaryFormatter();
        currentCompletedLevels = (int) bf.Deserialize(file);
        file.Close();
        return true;
    }

}
