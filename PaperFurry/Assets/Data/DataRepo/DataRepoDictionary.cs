using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


    [Serializable]
    public class DataRepo_KeyData
    {
        public string KeyData_Name;
        public string KeyData_stringValue;
        public float KeyData_floatValue;
    }

    [Serializable]
    public class DataRepo_DataServerMessage
    {
        public string Data_Name;
        public string Data_stringValue;
        public float Data_floatValue;
    }
    
    [Serializable]
    public class DataRepo_HistoryContorl
    {
        public string Data_Name;
        public string Data_stringValue;
        public float Data_floatValue;
    }

    [Serializable]
    public class DataRepo_CharacterInteractionPositionTrigger
    {
        public string Data_Name;
        public string Data_stringValue;
        public float Data_floatValue;
    }

    
public class DataRepoDictionary : MonoBehaviour
{
    // 声明一个字典，键是字符串，值是一个包含多个成员的类
    [SerializeField] public static Dictionary<string, DataRepo_KeyData> _DataRepo_KeyData;
    [SerializeField] public static Dictionary<string, DataRepo_HistoryContorl> _DataRepo_HistoryContorl;
    [SerializeField] public static Dictionary<string, DataRepo_CharacterInteractionPositionTrigger> _DataRepo_CharacterInteractionPositionTrigger;
    [SerializeField] public static Dictionary<string, DataRepo_DataServerMessage> _DataRepo_DataServerMessage;




    private void Awake()
    {
        // 初始化字典
        _DataRepo_KeyData = new Dictionary<string, DataRepo_KeyData>();
        _DataRepo_HistoryContorl = new Dictionary<string, DataRepo_HistoryContorl>();
        _DataRepo_CharacterInteractionPositionTrigger = new Dictionary<string, DataRepo_CharacterInteractionPositionTrigger>();
        _DataRepo_DataServerMessage = new Dictionary<string, DataRepo_DataServerMessage>();

/*         // 添加示例数据
        AddEntry("DataRepo_KeyData", "test", "TestValue", "string", -1f);
        AddEntry("DataRepo_HistoryContorl", "test", "TestValue", "string", -1f);
        AddEntry("DataRepo_CharacterInteractionPositionTrigger", "test", "TestValue", "string", -1f); */
    }

    // 添加字典条目的方法
    public void AddEntry(string DictionaryName,string ValueName, string ValueType = "normal", string stringValue = "null", float floatValue = -1f)
    {
        switch (DictionaryName)
        {
            case "DataRepo_KeyData":
                var dataKeyData = new DataRepo_KeyData { KeyData_Name = ValueType, KeyData_stringValue = stringValue, KeyData_floatValue = floatValue };
                _DataRepo_KeyData.TryAdd(ValueName, dataKeyData);
                break;
            case "DataRepo_HistoryContorl":
                var dataHistoryContorl = new DataRepo_HistoryContorl { Data_Name = ValueType, Data_stringValue = stringValue, Data_floatValue = floatValue };
                _DataRepo_HistoryContorl.TryAdd(ValueName, dataHistoryContorl);
                break;
            case "DataRepo_CharacterInteractionPositionTrigger":
                var dataCharacterInteractionPositionTrigger = new DataRepo_CharacterInteractionPositionTrigger { Data_Name = ValueType, Data_stringValue = stringValue, Data_floatValue = floatValue };
                _DataRepo_CharacterInteractionPositionTrigger.TryAdd(ValueName, dataCharacterInteractionPositionTrigger);
                break;
            case "DataRepo_DataServerMessage":
                var dataDataServerMessage = new DataRepo_DataServerMessage { Data_Name = ValueType, Data_stringValue = stringValue, Data_floatValue = floatValue };
                _DataRepo_DataServerMessage.TryAdd(ValueName, dataDataServerMessage);
                break;
            default:
                Debug.LogWarning("Unrecognized dictionary name.");
                break;
        }
    }
    public void RemoveEntry(string DictionaryName, string ValueName)
    {
        switch (DictionaryName)
        {
            case "DataRepo_KeyData":
                if (_DataRepo_KeyData.ContainsKey(ValueName))
                {
                    _DataRepo_KeyData.Remove(ValueName);
                }
                else
                {
                    Debug.LogWarning($"No entry with the key '{ValueName}' found to remove.");
                }
                break;
            case "DataRepo_HistoryContorl":
                if (_DataRepo_HistoryContorl.ContainsKey(ValueName))
                {
                    _DataRepo_HistoryContorl.Remove(ValueName);
                }
                else
                {
                    Debug.LogWarning($"No entry with the key '{ValueName}' found to remove.");
                }
                break;
            case "DataRepo_CharacterInteractionPositionTrigger":
                if (_DataRepo_CharacterInteractionPositionTrigger.ContainsKey(ValueName))
                {
                    _DataRepo_CharacterInteractionPositionTrigger.Remove(ValueName);
                }
                else
                {
                    Debug.LogWarning($"No entry with the key '{ValueName}' found to remove.");
                }
                break;
            case "DataRepo_DataServerMessage":
                if (_DataRepo_DataServerMessage.ContainsKey(ValueName))
                {
                    _DataRepo_DataServerMessage.Remove(ValueName);
                }
                else
                {
                    Debug.LogWarning($"No entry with the key '{ValueName}' found to remove.");
                }
                break;
            default:
                Debug.LogWarning("Unrecognized dictionary name.");
                break;
        }
    }

    public void test2printDictionary(string DictionaryName)
    {
        switch (DictionaryName)
        {
            case "DataRepo_KeyData":
                foreach (var item in _DataRepo_KeyData)
                {
                    Debug.Log(item.Key + " " + item.Value.KeyData_Name + " " + item.Value.KeyData_stringValue + " " + item.Value.KeyData_floatValue);
                }
               break;
            case "DataRepo_HistoryContorl":
                foreach (var item in _DataRepo_HistoryContorl)
                {
                    Debug.Log(item.Key + " " + item.Value.Data_Name + " " + item.Value.Data_stringValue + " " + item.Value.Data_floatValue);
                }
                break;
            case "DataRepo_CharacterInteractionPositionTrigger":
                foreach (var item in _DataRepo_CharacterInteractionPositionTrigger)
                {
                    Debug.Log(item.Key + " " + item.Value.Data_Name + " " + item.Value.Data_stringValue + " " + item.Value.Data_floatValue);
                }
                break;
            case "DataRepo_DataServerMessage":
                foreach (var item in _DataRepo_DataServerMessage)
                {
                    Debug.Log(item.Key + " " + item.Value.Data_Name + " " + item.Value.Data_stringValue + " " + item.Value.Data_floatValue);
                }
                break;
            default:
                Debug.LogWarning("Unrecognized dictionary name.");
                break;
        }
    }
}
