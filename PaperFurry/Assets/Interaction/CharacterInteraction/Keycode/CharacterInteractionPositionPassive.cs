using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色交互位置被动
/// 该代码用于非玩家角色(NPC)或联机玩家角色的位置操控
/// 请不要使用单例模式调用此代码(静态变量除外)，请使用字典传参
/// </summary>
public class CharacterInteractionPositionPassive : MonoBehaviour
{
    public enum PassiveMode
    {
        None,
        NPC,
        OnlinePlayer
    }

    public enum DictionaryChoice
    {
        None,
        DataRepo_DataServerMessage
    }    

    [SerializeField][ReadOnly]
    private Vector2 newPosition;
    [SerializeField]
    private PassiveMode mode;
    [SerializeField]
    private DictionaryChoice choice;

    [SerializeField]
    private string positionKeyName = "testCharacterPosition";

    //测试内容
    private DataServerDataConnection DataServerDataConnection;
    private void Start()
    {
        DataServerDataConnection = FindObjectOfType<DataServerDataConnection>();
    }

/*     float x=2f;
    float y=23f;
    private void Update()
    {
        //K键test_x减小，L键test_x增加
        if (Input.GetKeyDown(KeyCode.K))
        {
            x -= 0.05f; // 这里是减小的步长，可以根据需要调整
        }
        // L键按下时，test_x增加
        else if (Input.GetKeyDown(KeyCode.L))
        {
            x += 0.05f; // 这里是增加的步长，同样可以调整
        }
        Vector2 vector = transform.position;
        string vectorStr = vector.x.ToString("F2") + ";" + vector.y.ToString("F2");
        Debug.Log("Vector2 as string (custom format): " + vectorStr);

        string vectorStr = x.ToString("F3") + ";" + y.ToString("F3");
        
        DataServerDataConnection.AddMessage(positionKeyName,vectorStr); 
    } */

    private IEnumerator movePassive,getOnlineData;
    public void StartMove()
    {
        if(movePassive == null)
        {
            switch(mode)
            {
                case PassiveMode.NPC:
                    break;
                case PassiveMode.OnlinePlayer:
                    getOnlineData = GetOnlineData();
                    StartCoroutine(getOnlineData);
                    break;
            }
            movePassive = MovePassive();
            StartCoroutine(movePassive);
        }
        else
        {
            Debug.LogError("MovePassive is not null");
        }
    }

    public void StopMove()
    {
        StopAllCoroutines();
        movePassive = null;
        getOnlineData = null;
    }

    public void PositionSet(float x,float y)
    {
        transform.position = new Vector2(x,y);
    }

    private DataRepoDictionary DataRepoDictionary;
    private void Awake()
    {
        switch(choice)
        {
            case DictionaryChoice.None:
                break;
            case DictionaryChoice.DataRepo_DataServerMessage:
                DataRepoDictionary = FindObjectOfType<DataRepoDictionary>();
                break;
        }
    }

    private IEnumerator GetOnlineData()
    {
        if(choice == DictionaryChoice.DataRepo_DataServerMessage)
        {
            string get;
            string[] parts;
            while(true)
            {
                get = DataRepoDictionary._DataRepo_DataServerMessage[positionKeyName].Data_stringValue;
                parts = get.Split(';');
                newPosition = new Vector2(float.Parse(parts[0]),float.Parse(parts[1]));
                yield return null;
            }
        }
        else
        {
            Debug.LogError("未选择字典");
            yield return null;
        }
    }
    private IEnumerator MovePassive()
    {
        while(true)
        {
            transform.position = newPosition;
            yield return null;
        }
    }
}
