using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractionPositionTrigger : MonoBehaviour
{
    public enum InspectorOptions
    {
       None,
       NotAllowPass, //Not allow pass.
       AllowPass //Allow pass.
   }
   public enum RangeOptions
   {
       None,
       X,
       Y,
       XY
   }

   public enum BanArror
   {
       None,
       x_Left,
       x_Right,
       y_Up,
       y_Down
   }


   public enum ReturnCode
   {
      None,
      ReturnCodeStr1,
      ReturnCodeStr2,
      ReturnCodeStr3,
      ReturnCodeFloat1,
      ReturnCodeFloat2,
      ReturnCodeFloat3,
      ReturnFloor
   }

   public enum DictionaryChoice
   {
        None,
        DataRepo_CharacterInteractionPositionTrigger
   }
    public static string ReturnCodeStr1,ReturnCodeStr2,ReturnCodeStr3;
    public static float ReturnCodeFloat1,ReturnCodeFloat2,ReturnCodeFloat3;
    public static bool ReturnFloor;

    [Header("基础设置")]

    [SerializeField]
    private InspectorOptions Mode;
    [SerializeField]
    private RangeOptions Range_mode;
    [SerializeField]
    private BanArror Ban_arror;
    
    [Space]
    [SerializeField]
    private bool isSleep = true;    //默认超出范围就睡眠
    [SerializeField]
    private float sleepRange = 10f;   //睡眠范围
    [SerializeField]
    private float sleepTime = 1f;   //睡眠时间

    [Space][Space]
    [Header("返回值与检测范围")]
    [SerializeField]
    private ReturnCode ReturnCodeOption;
    [SerializeField]
    private string ReturnCodeValue;
    [SerializeField]
    private float range_x_left,range_x_right,range_y_up,range_y_down;

    [Space][Space]
    [Header("字典")]
    [SerializeField]
    private DictionaryChoice DictionaryChoiceOption;
    [SerializeField]
    private string DictionaryValueName;  //写入字典的键名
    [SerializeField]
    private string DictionaryValueType = "normal"; //写入字典的值类型
    [SerializeField]
    private string DictionaryValueString = "null";   //写入字典的值
    [SerializeField]
    private float DictionaryValueFloat = -1; //写入字典的值

    #pragma warning disable 0414
    [Space]
    [Header("状态")]
    [SerializeField][ReadOnly]
    private string status = "未启用";
    #pragma warning restore 0414
    //将警告消除

    [SerializeField][ReadOnly]
    private float left_edge,right_edge,up_edge,down_edge,distance; //仅供调试时面板查看

    //字典连接初始化
    //目前只支持字典添加键值
    private DataRepoDictionary DataRepoDictionary;
    private void Awake()
    {
        switch(DictionaryChoiceOption)
        {
            case DictionaryChoice.None:
                break;
            case DictionaryChoice.DataRepo_CharacterInteractionPositionTrigger:
                DataRepoDictionary = FindObjectOfType<DataRepoDictionary>();
                break;
        }
    }

    private IEnumerator startNotAllowPass,startAllowPass;
    private IEnumerator StartChecking()
    {
        
        switch(Mode)
        {
            case InspectorOptions.None:
                break;
            case InspectorOptions.NotAllowPass:
                status = "已启用";
                startNotAllowPass=StartNotAllowPass();
                StartCoroutine(startNotAllowPass);
                break;
            case InspectorOptions.AllowPass:
                status = "已启用";
                startAllowPass=StartAllowPass();
                StartCoroutine(startAllowPass);
                break;
        }
        yield return null;
    }
    private IEnumerator StartNotAllowPass()
    {
        switch(Range_mode)
        {
            case RangeOptions.None:
                break;
            case RangeOptions.X:
                while(true)
                {
                    left_edge = transform.position.x-range_x_left;
                    right_edge = transform.position.x+range_x_right;
                    up_edge = transform.position.y+range_y_up;
                    down_edge = transform.position.y-range_y_down;
                    distance = Mathf.Abs(transform.position.x-CharacterInteractionPosition.position_now_x);

                    //睡眠监测
                    if(isSleep && distance > sleepRange)
                    {
                        status = "睡眠中";
                        yield return new WaitForSeconds(sleepTime);
                    }

                    if(left_edge < CharacterInteractionPosition.position_now_x && right_edge > CharacterInteractionPosition.position_now_x)
                    {
                        status = "触发中";
                        switch(ReturnCodeOption)
                        {
                            case ReturnCode.None:
                                break;
                            case ReturnCode.ReturnCodeStr1:
                                ReturnCodeStr1 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr2:
                                ReturnCodeStr2 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr3:
                                ReturnCodeStr3 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeFloat1:
                                ReturnCodeFloat1 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat2:
                                ReturnCodeFloat2 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat3:
                                ReturnCodeFloat3 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnFloor:
                                ReturnFloor = true;
                                break;
                        }
                        
                        switch(DictionaryChoiceOption)
                        {
                            case DictionaryChoice.None:
                                break;
                            case DictionaryChoice.DataRepo_CharacterInteractionPositionTrigger:
                                DataRepoDictionary.AddEntry("DataRepo_CharacterInteractionPositionTrigger",DictionaryValueName,DictionaryValueType,DictionaryValueString,DictionaryValueFloat);
                                break;
                            default:
                                Debug.LogWarning("Unrecognized dictionary name.");
                                break;
                        }
                        switch(Ban_arror)
                        {
                            case BanArror.None:
                                break;
                            case BanArror.x_Left:
                                CharacterInteractionPosition.axis_x_left = false;
                                while (left_edge < CharacterInteractionPosition.position_now_x && right_edge > CharacterInteractionPosition.position_now_x)
                                {
                                    yield return new WaitForFixedUpdate();
                                }
                                CharacterInteractionPosition.axis_x_left = true;
                                break;
                            case BanArror.x_Right:
                                CharacterInteractionPosition.axis_x_right = false;
                                while (left_edge < CharacterInteractionPosition.position_now_x && right_edge > CharacterInteractionPosition.position_now_x)
                                {
                                    yield return new WaitForFixedUpdate();
                                }
                                CharacterInteractionPosition.axis_x_right = true;
                                break;
                            case BanArror.y_Up:
                                Debug.LogError("错误: 检测此轴不可以限制另外一轴");
                                break;
                            case BanArror.y_Down:
                                Debug.LogError("错误: 检测此轴不可以限制另外一轴");
                                break;
                            default:
                                Debug.LogError("错误: 未定义的BanArror选项");
                                break;
                        }
                    }
                    else
                    {
                        status = "未触发";
                    }
    	            yield return new WaitForFixedUpdate();
                }
            case RangeOptions.Y:
                while(true)
                {
                    left_edge = transform.position.x-range_x_left;
                    right_edge = transform.position.x+range_x_right;
                    up_edge = transform.position.y+range_y_up;
                    down_edge = transform.position.y-range_y_down;
                    distance = Mathf.Abs(transform.position.y-CharacterInteractionPosition.position_now_y);

                    //睡眠监测
                    if(isSleep && distance > sleepRange)
                    {
                        status = "睡眠中";
                        yield return new WaitForSeconds(sleepTime);
                    }

                    if(up_edge > CharacterInteractionPosition.position_now_y && down_edge < CharacterInteractionPosition.position_now_y)
                    {
                        status = "触发中";
                        switch(ReturnCodeOption)
                        {
                            case ReturnCode.None:
                                break;
                            case ReturnCode.ReturnCodeStr1:
                                ReturnCodeStr1 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr2:
                                ReturnCodeStr2 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr3:
                                ReturnCodeStr3 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeFloat1:
                                ReturnCodeFloat1 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat2:
                                ReturnCodeFloat2 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat3:
                                ReturnCodeFloat3 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnFloor:
                                ReturnFloor = true;
                                break;
                        }
                        switch(DictionaryChoiceOption)
                        {
                            case DictionaryChoice.None:
                                break;
                            case DictionaryChoice.DataRepo_CharacterInteractionPositionTrigger:
                                DataRepoDictionary.AddEntry("DataRepo_CharacterInteractionPositionTrigger",DictionaryValueName,DictionaryValueType,DictionaryValueString,DictionaryValueFloat);
                                break;
                            default:
                                Debug.LogWarning("Unrecognized dictionary name.");
                                break;
                        }
                        switch(Ban_arror)
                        {
                            case BanArror.None:
                                break;
                            case BanArror.y_Up:
                                CharacterInteractionPosition.axis_y_up = false;
                                while (up_edge > CharacterInteractionPosition.position_now_y && down_edge < CharacterInteractionPosition.position_now_y)
                                {
                                    yield return new WaitForFixedUpdate();
                                }
                                CharacterInteractionPosition.axis_y_up = true;
                                break;
                            case BanArror.y_Down:
                                CharacterInteractionPosition.axis_y_down = false;
                                while (up_edge > CharacterInteractionPosition.position_now_y && down_edge < CharacterInteractionPosition.position_now_y)
                                {
                                    yield return new WaitForFixedUpdate();
                                }
                                CharacterInteractionPosition.axis_y_down = true;
                                break;
                            case BanArror.x_Left:
                                Debug.LogError("错误: 检测此轴不可以限制另外一轴");
                                break;
                            case BanArror.x_Right:
                                Debug.LogError("错误: 检测此轴不可以限制另外一轴");
                                break;
                            default:
                                Debug.LogError("错误: 未定义的BanArror选项");
                                break;
                        }

                    }
                    else
                    {
                        status = "未触发";
                    }
    	            yield return new WaitForFixedUpdate();
                }
            case RangeOptions.XY:
                //注释掉了这个警告
                //Debug.LogWarning("不推荐在启用xy轴检测的情况下,启用对某一特定方向的限制\n可以使用重复使用两次脚本或新建物质以实现对两个方向的同时限制\n距离采用平方表示未开方");
                while(true)
                {
                    left_edge = transform.position.x-range_x_left;
                    right_edge = transform.position.x+range_x_right;
                    up_edge = transform.position.y+range_y_up;
                    down_edge = transform.position.y-range_y_down;
                    distance = (transform.position.x - CharacterInteractionPosition.position_now_x) * (transform.position.x - CharacterInteractionPosition.position_now_x) + 
                       (transform.position.y - CharacterInteractionPosition.position_now_y) * (transform.position.y - CharacterInteractionPosition.position_now_y);

                    //睡眠监测
                    if(isSleep && distance > sleepRange * sleepRange)   //特殊平方处理
                    {
                        status = "睡眠中";
                        yield return new WaitForSeconds(sleepTime);
                    }

                    if(left_edge < CharacterInteractionPosition.position_now_x && transform.position.x+range_x_right > CharacterInteractionPosition.position_now_x && up_edge > CharacterInteractionPosition.position_now_y && down_edge < CharacterInteractionPosition.position_now_y)
                    {
                        status = "触发中";
                        switch(ReturnCodeOption)
                        {
                            case ReturnCode.None:
                                break;
                            case ReturnCode.ReturnCodeStr1:
                                ReturnCodeStr1 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr2:
                                ReturnCodeStr2 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr3:
                                ReturnCodeStr3 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeFloat1:
                                ReturnCodeFloat1 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat2:
                                ReturnCodeFloat2 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat3:
                                ReturnCodeFloat3 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnFloor:
                                ReturnFloor = true;
                                break;
                        }
                        switch(DictionaryChoiceOption)
                        {
                            case DictionaryChoice.None:
                                break;
                            case DictionaryChoice.DataRepo_CharacterInteractionPositionTrigger:
                                DataRepoDictionary.AddEntry("DataRepo_CharacterInteractionPositionTrigger",DictionaryValueName,DictionaryValueType,DictionaryValueString,DictionaryValueFloat);
                                break;
                            default:
                                Debug.LogWarning("Unrecognized dictionary name.");
                                break;
                        }
                        switch(Ban_arror)
                        {
                            case BanArror.None:
                                break;
                            case BanArror.x_Left:
                                CharacterInteractionPosition.axis_x_left = false;
                                while (left_edge < CharacterInteractionPosition.position_now_x && right_edge > CharacterInteractionPosition.position_now_x)
                                {
                                    yield return new WaitForFixedUpdate();
                                }
                                CharacterInteractionPosition.axis_x_left = true;
                                break;
                            case BanArror.x_Right:
                                CharacterInteractionPosition.axis_x_right = false;
                                while (left_edge < CharacterInteractionPosition.position_now_x && right_edge > CharacterInteractionPosition.position_now_x)
                                {
                                    yield return new WaitForFixedUpdate();
                                }
                                CharacterInteractionPosition.axis_x_right = true;
                                break;
                            case BanArror.y_Up:
                                CharacterInteractionPosition.axis_y_up = false;
                                while (up_edge > CharacterInteractionPosition.position_now_y && down_edge < CharacterInteractionPosition.position_now_y)
                                {
                                    yield return new WaitForFixedUpdate();
                                }
                                CharacterInteractionPosition.axis_y_up = true;
                                break;
                            case BanArror.y_Down:
                                CharacterInteractionPosition.axis_y_down = false;
                                while (up_edge > CharacterInteractionPosition.position_now_y && down_edge < CharacterInteractionPosition.position_now_y)
                                {
                                    yield return new WaitForFixedUpdate();
                                }
                                CharacterInteractionPosition.axis_y_down = true;
                                break;
                            default:
                                Debug.LogError("错误: 未定义的BanArror选项");
                                break;
                        }
                    }
                    else
                    {
                        status = "未触发";
                    }
                    yield return new WaitForFixedUpdate();
                }
        }
        yield return null;
    }
    private IEnumerator StartAllowPass()
    {
        //我发现一个惊天大秘密
        //只要我Ban_Arror选为null,NotAllowPass就是可以穿过的触发器
        //这个分类还是保留着吧万一有变动你说对吧

        switch(Ban_arror)
        {
            case BanArror.None:
                break;
            default:
                Debug.LogError("触发器不能选择限制移动");
                break;
        }
        
        switch(Range_mode)
        {
            case RangeOptions.None:
                break;
            case RangeOptions.X:
                while(true)
                {
                    left_edge = transform.position.x-range_x_left;
                    right_edge = transform.position.x+range_x_right;
                    up_edge = transform.position.y+range_y_up;
                    down_edge = transform.position.y-range_y_down;
                    distance = Mathf.Abs(transform.position.x-CharacterInteractionPosition.position_now_x);

                    //睡眠监测
                    if(isSleep && distance > sleepRange)
                    {
                        status = "睡眠中";
                        yield return new WaitForSeconds(sleepTime);
                    }

                    if(left_edge < CharacterInteractionPosition.position_now_x && right_edge > CharacterInteractionPosition.position_now_x)
                    {
                        status = "触发中";
                        switch(ReturnCodeOption)
                        {
                            case ReturnCode.None:
                                break;
                            case ReturnCode.ReturnCodeStr1:
                                ReturnCodeStr1 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr2:
                                ReturnCodeStr2 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr3:
                                ReturnCodeStr3 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeFloat1:
                                ReturnCodeFloat1 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat2:
                                ReturnCodeFloat2 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat3:
                                ReturnCodeFloat3 = float.Parse(ReturnCodeValue);
                                break;
                        }
                        switch(DictionaryChoiceOption)
                        {
                            case DictionaryChoice.None:
                                break;
                            case DictionaryChoice.DataRepo_CharacterInteractionPositionTrigger:
                                DataRepoDictionary.AddEntry("DataRepo_CharacterInteractionPositionTrigger",DictionaryValueName,DictionaryValueType,DictionaryValueString,DictionaryValueFloat);
                                break;
                            default:
                                Debug.LogWarning("Unrecognized dictionary name.");
                                break;
                        }
                    }
                    else
                    {
                        status = "未触发";
                    }
    	            yield return new WaitForFixedUpdate();
                }
            case RangeOptions.Y:
                while(true)
                {
                    left_edge = transform.position.x-range_x_left;
                    right_edge = transform.position.x+range_x_right;
                    up_edge = transform.position.y+range_y_up;
                    down_edge = transform.position.y-range_y_down;
                    distance = Mathf.Abs(transform.position.y-CharacterInteractionPosition.position_now_y);

                    //睡眠监测
                    if(isSleep && distance > sleepRange)
                    {
                        status = "睡眠中";
                        yield return new WaitForSeconds(sleepTime);
                    }

                    if(up_edge > CharacterInteractionPosition.position_now_y && down_edge < CharacterInteractionPosition.position_now_y)
                    {
                        status = "触发中";
                        switch(ReturnCodeOption)
                        {
                            case ReturnCode.None:
                                break;
                            case ReturnCode.ReturnCodeStr1:
                                ReturnCodeStr1 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr2:
                                ReturnCodeStr2 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr3:
                                ReturnCodeStr3 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeFloat1:
                                ReturnCodeFloat1 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat2:
                                ReturnCodeFloat2 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat3:
                                ReturnCodeFloat3 = float.Parse(ReturnCodeValue);
                                break;
                        }
                        switch(DictionaryChoiceOption)
                        {
                            case DictionaryChoice.None:
                                break;
                            case DictionaryChoice.DataRepo_CharacterInteractionPositionTrigger:
                                DataRepoDictionary.AddEntry("DataRepo_CharacterInteractionPositionTrigger",DictionaryValueName,DictionaryValueType,DictionaryValueString,DictionaryValueFloat);
                                break;
                            default:
                                Debug.LogWarning("Unrecognized dictionary name.");
                                break;
                        }
                    }
                    else
                    {
                        status = "未触发";
                    }
    	            yield return new WaitForFixedUpdate();
                }
            case RangeOptions.XY:
                //Debug.LogWarning("不推荐在启用xy轴检测的情况下,启用对某一特定方向的限制\n可以使用重复使用两次脚本或新建物质以实现对两个方向的同时限制\n距离采用平方表示未开方");
                while(true)
                {
                    left_edge = transform.position.x-range_x_left;
                    right_edge = transform.position.x+range_x_right;
                    up_edge = transform.position.y+range_y_up;
                    down_edge = transform.position.y-range_y_down;
                    distance = (transform.position.x - CharacterInteractionPosition.position_now_x) * (transform.position.x - CharacterInteractionPosition.position_now_x) + 
                       (transform.position.y - CharacterInteractionPosition.position_now_y) * (transform.position.y - CharacterInteractionPosition.position_now_y);

                    //睡眠监测
                    if(isSleep && distance > sleepRange * sleepRange)   //特殊平方处理
                    {
                        status = "睡眠中";
                        yield return new WaitForSeconds(sleepTime);
                    }

                    if(left_edge < CharacterInteractionPosition.position_now_x && right_edge > CharacterInteractionPosition.position_now_x && up_edge > CharacterInteractionPosition.position_now_y && down_edge < CharacterInteractionPosition.position_now_y)
                    {
                        status = "触发中";
                        switch(ReturnCodeOption)
                        {
                            case ReturnCode.None:
                                break;
                            case ReturnCode.ReturnCodeStr1:
                                ReturnCodeStr1 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr2:
                                ReturnCodeStr2 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeStr3:
                                ReturnCodeStr3 = ReturnCodeValue;
                                break;
                            case ReturnCode.ReturnCodeFloat1:
                                ReturnCodeFloat1 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat2:
                                ReturnCodeFloat2 = float.Parse(ReturnCodeValue);
                                break;
                            case ReturnCode.ReturnCodeFloat3:
                                ReturnCodeFloat3 = float.Parse(ReturnCodeValue);
                                break;
                        }
                        switch(DictionaryChoiceOption)
                        {
                            case DictionaryChoice.None:
                                break;
                            case DictionaryChoice.DataRepo_CharacterInteractionPositionTrigger:
                                DataRepoDictionary.AddEntry("DataRepo_CharacterInteractionPositionTrigger",DictionaryValueName,DictionaryValueType,DictionaryValueString,DictionaryValueFloat);
                                break;
                            default:
                                Debug.LogWarning("Unrecognized dictionary name.");
                                break;
                        }
                    }
                    else
                    {
                        status = "未触发";
                    }
                    yield return new WaitForFixedUpdate();
                }
        }
        yield return null;
    }




    public void CheckOn()
    {
        if(startNotAllowPass == null || startAllowPass == null)
        {
            status = "正在启用";
            StartCoroutine(StartChecking());
        }
        else
        {
            Debug.LogError("CheckOn: startNotAllowPass,startAllowPass is not null");
            status = "异常";
        }
    }
    public void CheckOff()
    {
        if(startNotAllowPass != null)
        {
            StopCoroutine(startNotAllowPass);
            status = "已关闭";
        }
        else if(startAllowPass != null)
        {
            StopCoroutine(startAllowPass);
            status = "已关闭";
        }
        else
        {
            Debug.LogError("CheckOff: startNotAllowPass, startAllowPass is null");
            status = "异常";
        }
    }

    void Start()
    {
        CheckOn();
    }
}