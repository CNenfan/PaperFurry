using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 使用命令：
/// 0. 初始化:    private DataSaveSystem DataSaveSystem; DataSaveSystem = FindObjectOfType<DataSaveSystem>();
/// 1. 创建存档：DataSaveSystem.SaveWrite(string save_name);
/// 2. 加载存档：DataSaveSystem.SaveLoad(string save_name);
/// 3. 删除存档（未实装）：DataSaveSystem.DeleteSave(string save_name);

public class TestDataSaveSystem : MonoBehaviour
{
    private DataSaveSystem DataSaveSystem;
    private DataRepoDictionary DataRepoDictionary;

/*      void Start()
    {
        DataSaveSystem = new DataSaveSystem();
        DataSaveSystem.SaveLoad("Test_Save_Love_From_Enfan");
    } */
    
//注意！！！测试代码引用了CharacterInteractionPositionTrigger，由于程序集，现在使用会报错
/*     void Start()
    {
        StartCoroutine(Test2Save());
    }
    IEnumerator Test2Save()
    {
        DataSaveSystem = FindObjectOfType<DataSaveSystem>();
        DataRepoDictionary = FindObjectOfType<DataRepoDictionary>();
        while(true)
        {
            
            if(CharacterInteractionPositionTrigger.ReturnCodeFloat1 == 114514f)
            {
                DataRepoDictionary.test2printDictionary("DataRepo_CharacterInteractionPositionTrigger");
                DataSaveSystem.SaveWrite("Test_Save_Love_From_Enfan");
                
                break;
            }
            yield return new WaitForSeconds(1);
        }
    } */

    IEnumerator Test2Load()
    {
        DataSaveSystem = FindObjectOfType<DataSaveSystem>();
        DataRepoDictionary = FindObjectOfType<DataRepoDictionary>();
        DataSaveSystem.SaveLoad("Test_Save_Love_From_Enfan");
        DataRepoDictionary.test2printDictionary("DataRepo_CharacterInteractionPositionTrigger");
        yield return null;
    }
}
