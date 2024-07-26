using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System.Security.Cryptography;

/// <summary>
/// 修改存档内容时，搜索#号定位到需要修改处
/// 
/// 使用命令：
/// 0. 初始化:    private DataSaveSystem DataSaveSystem; DataSaveSystem = FindObjectOfType<DataSaveSystem>();
/// 1. 创建存档：DataSaveSystem.SaveWrite(string save_name);
/// 2. 加载存档：DataSaveSystem.SaveLoad(string save_name);
/// 3. 删除存档（未实装）：DataSaveSystem.DeleteSave(string save_name);
/// </summary>

//存档数据结构
[System.Serializable]   //可被序列化
public class DataSave
{
    //#在此处修改创建数据

    //存档基本属性
    public string save_name = "New_Save";
    public string save_time = "2007-12-02 00:00:00";
    //--------------------------------------------------------------------------------
    //字典存储
    public Dictionary<string, DataRepo_KeyData> _DataRepo_KeyData;
    public Dictionary<string, DataRepo_HistoryContorl> _DataRepo_HistoryContorl;
    public Dictionary<string, DataRepo_CharacterInteractionPositionTrigger> _DataRepo_CharacterInteractionPositionTrigger;
    //--------------------------------------------------------------------------------

}

[System.Serializable]
public class DataSaveSystem : MonoBehaviour
{
    private const int KeySize = 256; // AES key size in bits
    private const int IvSize = 128; // IV size in bits for CBC mode

    // Generates a specified number of random bytes using a secure cryptographic random number generator.
    private static byte[] GenerateRandomBytes(int size)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            var bytes = new byte[size];
            rng.GetBytes(bytes);
            return bytes;
        }
    }

    // Encrypts plaintext using the given key and IV with AES-256-CBC mode.
    private static byte[] Encrypt(byte[] plaintext, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plaintext, 0, plaintext.Length);
                }

                return ms.ToArray();
            }
        }
    }

    // Decrypts ciphertext using the given key and IV with AES-256-CBC mode.
    private static byte[] Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            using (var ms = new MemoryStream(ciphertext))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var output = new MemoryStream())
            {
                cs.CopyTo(output);
                return output.ToArray();
            }
        }
    }


    //上面是加密核心代码，下面是存档创建和读取代码
    
    //保存存档获取游戏中数据
    public DataSave CreateSave(string save_name)
    { 
        DataSave save = new DataSave(); //创建一个Save对象存储当前游戏数据

        //#在此处修改创建数据

        //存档基本属性
            save.save_name = save_name;

            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yyyy-MM-dd HH:mm:ss"); // 格式化时间字符串
            save.save_time = formattedTime;
        //--------------------------------------------------------------------------------
        //字典存储

            save._DataRepo_KeyData = DataRepoDictionary._DataRepo_KeyData;
            save._DataRepo_HistoryContorl = DataRepoDictionary._DataRepo_HistoryContorl;
            save._DataRepo_CharacterInteractionPositionTrigger = DataRepoDictionary._DataRepo_CharacterInteractionPositionTrigger;
        //--------------------------------------------------------------------------------
        return save;
    }
    
    // Writes an encrypted save to disk, storing the key and IV alongside the encrypted data in a single file.
    public void SaveWrite(string save_name, string path = null)
    {
        DataSave save = CreateSave(save_name);

        // Generate a random key and IV for this save.
        byte[] key = GenerateRandomBytes(KeySize / 8);
        byte[] iv = GenerateRandomBytes(IvSize / 8);

        // Serialize the save object and encrypt it using the generated key and IV.
        BinaryFormatter bf = new BinaryFormatter();
        byte[] serializedData;
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, save);
            serializedData = ms.ToArray();
        }
        byte[] encryptedData = Encrypt(serializedData, key, iv);

        if(path==null)
        {
            path = Application.persistentDataPath+"/DataSave";
        }
        //默认储存点：Application.persistentDataPath+"/DataSave"
        //就是%AppData%\LocalLow\Team EtherArc\RF-Memories from FurCafe

        // Write the encrypted data to a file.
        string dataFilePath = $"{path}/{save_name}.save.rf";
        File.WriteAllBytes(dataFilePath, encryptedData);

        // Combine the key and IV into a single byte array and write it to a file.
        string keyFilePath = $"{path}/{save_name}.key.save.rf";
        using (var combinedKeyIvStream = new MemoryStream())
        {
            combinedKeyIvStream.Write(key, 0, key.Length);
            combinedKeyIvStream.Write(iv, 0, iv.Length);
            File.WriteAllBytes(keyFilePath, combinedKeyIvStream.ToArray());
        }

        Debug.Log($"Encrypted save '{save_name}' written with key and IV stored in '{keyFilePath}'.");
    }

    // Loads an encrypted save from disk, extracting the key and IV from the same file as the encrypted data.
    public void SaveLoad(string save_name, string path = null)
    {
        if(path==null)
        {
            path = Application.persistentDataPath+"/DataSave";
        }

        string dataFilePath = $"{path}/{save_name}.save.rf";
        string keyFilePath = $"{path}/{save_name}.key.save.rf";

        if (File.Exists(dataFilePath) && File.Exists(keyFilePath))
        {
            // Read the combined key and IV from the file.
            byte[] combinedKeyIv = File.ReadAllBytes(keyFilePath);

            // Split the combined key and IV into separate arrays.
            byte[] key = new byte[KeySize / 8];
            byte[] iv = new byte[IvSize / 8];
            Buffer.BlockCopy(combinedKeyIv, 0, key, 0, key.Length);
            Buffer.BlockCopy(combinedKeyIv, key.Length, iv, 0, iv.Length);

            // Read the encrypted data from the file.
            byte[] encryptedData = File.ReadAllBytes(dataFilePath);

            // Decrypt the data using the extracted key and IV.
            byte[] decryptedData = Decrypt(encryptedData, key, iv);

            // Deserialize the decrypted data into a DataSave object.
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream(decryptedData))
            {
                //#在此处修改读取数据
                DataSave save = (DataSave)bf.Deserialize(ms);

                //字典读取
                    DataRepoDictionary._DataRepo_KeyData = save._DataRepo_KeyData ;
                    DataRepoDictionary._DataRepo_HistoryContorl = save._DataRepo_HistoryContorl;
                    DataRepoDictionary._DataRepo_CharacterInteractionPositionTrigger = save._DataRepo_CharacterInteractionPositionTrigger;
                //--------------------------------------------------------------------------------
                Debug.Log($"Loaded save '{save_name}':");
                Debug.Log($"  Name: {save.save_name}");
                Debug.Log($"  Time: {save.save_time}");

            }
        }
        else
        {
            Debug.LogError("Data or key not found.");
        }
    }
}

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//旧的代码，无加密功能
/* public class DataSaveSystemUnencrypted
{
    //保存存档获取游戏中数据
    public DataSave CreateSave(string save_name){ //创建一个Save对象存储当前游戏数据
        DataSave save = new DataSave();

        //存档基本属性
            save.save_name = save_name;

            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("yyyy-MM-dd HH:mm:ss"); // 格式化时间字符串
            save.save_time = formattedTime;
        //--------------------------------------------------------------------------------
        return save;
    }

    //保存存档
    public void SaveWrite(string save_name)
    {
        DataSave save=CreateSave(save_name);
        //获取当前的游戏数据存在Save对象里
        BinaryFormatter bf=new BinaryFormatter();
        //创建一个二进制形式

        string path = Application.persistentDataPath+"/DataSave";
        if (!Directory.Exists(path)) // 检查目录是否存在
        {
            Directory.CreateDirectory(path); // 如果不存在，创建目录
        }
        FileStream fs = File.Create(path+"/"+save.save_name+".save.rf");
        //这里指使用持久路径创建一个文件流并将其保存在Data里
        //由于持久路径在Windows系统是隐藏的，所以无法找到Data本身
        //如果想看到，可以改成dataPath
        //文件后缀可以随便改，甚至是自定义的

        
        bf.Serialize(fs,save);
        //将Save对象转化为字节
        fs.Close();
        //关文件流
        Debug.Log("已存档:"+save.save_name+" 存档时间:"+save.save_time);
    }

    public void SaveLoad(string save_name){
        if(File.Exists(Application.persistentDataPath+"/DataSave/"+save_name+".save.rf"))
        //判断文件是否创建
        {
            BinaryFormatter bf=new BinaryFormatter();
            FileStream fs=File.Open(Application.persistentDataPath+"/DataSave/"+save_name+".save.rf",FileMode.Open);//打开文件
            DataSave save=bf.Deserialize(fs) as DataSave;
            //反序列化并将数据储存至save（因为返回变量类型不对，所以要强制转换为DataSave类

            fs.Close();
            //关文件流

            Debug.Log("Load Save:"+save.save_name);//输出
            Debug.Log("Load Time:"+save.save_time);
            //赋值
        }else
        {
            Debug.LogError("Data Not Found");
        }
    }
} */
//https://zhuanlan.zhihu.com/p/405194677