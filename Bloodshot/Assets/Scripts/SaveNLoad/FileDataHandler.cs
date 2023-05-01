
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Mono.Cecil.Cil;

public class FileDataHandler 
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    private bool _encryptData = false;
    private string _codeWord = "COWBELLGOTH"; 

    public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
        _encryptData = encryptData;
    }
     
    public void Save(GameData data)
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (_encryptData)
                dataToStore = EncryptDecrypt(dataToStore);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }

        catch(Exception e)
        {
            Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
        }
    }
    
    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (_encryptData)
                    dataToLoad = EncryptDecrypt(dataToLoad);

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }

            catch(Exception e)
            { 
                Debug.LogError("Error on trying to load data from file: "+fullPath+"\n"+e); 
            }
        }

        return loadData;
    } 

    public void Delete()
    {
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _codeWord[i % _codeWord.Length]);
        }

        return modifiedData;
    }
}
