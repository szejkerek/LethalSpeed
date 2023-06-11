using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class JsonDataService : IDataService
{
    private string KEY = "LsS/KatJ5yue1nb5N31Y0R9GsHWEDTJDACbocL8Ee9E=";
    private string IV = "lxHpSqDH94tbbdJFymlsXw==";
    public void SaveData<T>(string RelativePath, T Data, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;
        string encryptedRelativeDataPath = RelativePath.Insert(RelativePath.IndexOf("/") + 1, "encrypted-");
        string encryptedDataPath = Application.persistentDataPath + encryptedRelativeDataPath;

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (Encrypted) 
            {
                using FileStream stream = File.Create(encryptedDataPath);
                WriteEncrypedData(Data, stream);
                stream.Close();
            }
            else 
            {
                using FileStream stream = File.Create(path);
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(Data));
            }
        }
        catch(Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            throw e;
        }

    }

    private void WriteEncrypedData<T> (T Data, FileStream Stream)
    {
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(
            Stream,
            cryptoTransform,
            CryptoStreamMode.Write
        );
        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Data)));
    }
    public T LoadData<T>(string RelativePath, bool Encrypted)
    {
        string path = Application.persistentDataPath + RelativePath;
        string encryptedRelativeDataPath = RelativePath.Insert(RelativePath.IndexOf("/") + 1, "encrypted-");
        string encryptedDataPath = Application.persistentDataPath + encryptedRelativeDataPath;

        try
        {
            T data;
            if (Encrypted)
            {
                if (!File.Exists(encryptedDataPath))
                {
                    Debug.LogError($"Cannot Load file at {encryptedDataPath}. File does not exist!");
                    throw new FileNotFoundException($"{encryptedDataPath} does not exist!");
                }
                data = ReadEncryptedData<T>(encryptedDataPath);
            }
            else
            {
                if (!File.Exists(path))
                {
                    Debug.LogError($"Cannot Load file at {path}. File does not exist!");
                    throw new FileNotFoundException($"{path} does not exist!");
                }
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            return data;
        }
        catch (Exception e) 
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}"); 
            throw e;
        }
    }

    private T ReadEncryptedData<T>(string Path)
    {
        byte[] fileBytes = File.ReadAllBytes(Path);
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
            aesProvider.Key, 
            aesProvider.IV
        );
        using MemoryStream decryptionStream = new MemoryStream(fileBytes);
        using CryptoStream cryptoStream = new CryptoStream(
            decryptionStream,
            cryptoTransform,
            CryptoStreamMode.Read
        );
        using StreamReader reader = new StreamReader(cryptoStream);
        string result = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(result);
    }
}
