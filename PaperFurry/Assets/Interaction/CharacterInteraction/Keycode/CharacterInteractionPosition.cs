using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterInteractionPosition : MonoBehaviour
{
    public static float position_now_x,position_now_y,position_now_z;
    //默认开启左右移动，不开启上下移动，开启跳跃，跳跃模式为伪跳跃，无地板监测
    //开启地板检测请将jumpMode设置为false
    public static bool axis_x_left = true,axis_x_right = true,axis_y_up = false,axis_y_down = false,axis_jump = true,jumpMode = true;
    public float speed = 1f,speed_max_x = 1f,speed_max_y = 1f;

    public void PositionSet(float input_x,float input_y = 0,float input_z = 0)
    {
        CharacterInteractionPosition.position_now_x = input_x;
        CharacterInteractionPosition.position_now_y = input_y;
        CharacterInteractionPosition.position_now_z = input_z;
        transform.position = new Vector3(input_x,input_y,input_z);
    }

    public KeyCode KeyCodeUp = KeyCode.W;
    public KeyCode KeyCodeDown = KeyCode.S;
    public KeyCode KeyCodeLeft = KeyCode.A;
    public KeyCode KeyCodeRight = KeyCode.D;
    public KeyCode KeyCodeJump = KeyCode.Space;

    [Header("轴状态")]
    [SerializeField][ReadOnly]
    private bool x_left = axis_x_left;
    [SerializeField][ReadOnly]
    private bool x_right = axis_x_right;
    [SerializeField][ReadOnly]
    private bool y_up = axis_y_up;
    [SerializeField][ReadOnly]
    private bool y_down = axis_y_down;
    [SerializeField][ReadOnly]
    private bool y_jump = axis_jump;
    [SerializeField][ReadOnly]
    private string y_jump_mode = jumpMode ? "伪跳跃" : "检测地板跳跃";


    public void KeyRegister(string code,KeyCode key)
    {
        switch (code)
        {
            case "up":
            {
                KeyCodeUp = key;
                break;
            }
            case "down":
            {
                KeyCodeDown = key;
                break;
            }
            case "left":
            {
                KeyCodeLeft = key;
                break;
            }
            case "right":
            {
                KeyCodeRight = key;
                break;
            }
            case "jump":
            {
                KeyCodeJump = key;
                break;
            }
            default:
            {
                Debug.LogError("KeyRegister:code is not found");
                break;
            }
        }
    }
    private Coroutine currentMoveCoroutine;
    //监听键盘对角色进行移动
    public void PositionMove(string mode,float input1 = 1f)
    {
        switch (mode)
        {
            case "on":
            {
                if (currentMoveCoroutine != null)
                {
                    StopCoroutine(PositionMoveContinueIEnumerator());
                }
                // 启动协程
                currentMoveCoroutine = StartCoroutine(PositionMoveContinueIEnumerator());
                break;
            }
            case "off":
            {
                StopCoroutine(PositionMoveContinueIEnumerator());
                break;
            }
            case "speed":
            {
                speed = input1;
                break;
            }
            case "speed_max_x":
            {
                speed_max_x = input1;
                break;
            }
            case "speed_max_y":
            {
                speed_max_y = input1;
                break;
            }
            case "jumpForce":
            {
                jumpForce = input1;
                break;
            }
            case "gravity":
            {
                gravity = input1;
                break;
            }
            case "axis_x_left":
            {
                axis_x_left = input1 == 1 ? true : false;
                break;
            }
            case "axis_x_right":
            {
                axis_x_right = input1 == 1 ? true : false;
                break;
            }
            case "axis_y_up":
            {
                axis_y_up = input1 == 1 ? true : false;
                break;
            }
            case "axis_y_down":
            {
                axis_y_down = input1 == 1 ? true : false;
                break;
            }
            case "axis_jump":
            {
                axis_jump = input1 == 1 ? true : false;
                break;
            }
            case "jumpMode":
            {
                jumpMode = input1 == 1 ? true : false;
                break;
            }
            default:
            {
                Debug.LogError("CharacterInteractionPosition.PositionMove 模式 传参错误");
                break;                
            }

        }

        //在检查器处刷新
        x_left = axis_x_left;
        x_right = axis_x_right;
        y_up = axis_y_up;
        y_down = axis_y_down;
        y_jump = axis_jump;
        y_jump_mode = jumpMode ? "伪跳跃" : "检测地板跳跃";
    }


    private Vector3 newPosition = Vector3.zero;

    //跳跃
    [HideInInspector]public bool isJumping = false;
    public float jumpForce = 0.1f; // 跳跃力
    private float jumpForce_,original_y;
    public float gravity = 0.005f;
    //获取地板状态初始化
    private CharacterInteractionPositionTrigger CharacterInteractionPositionTrigger;
    void Awake()
    {
        CharacterInteractionPositionTrigger = FindObjectOfType<CharacterInteractionPositionTrigger>();
    }

    private IEnumerator PositionMoveContinueIEnumerator()
    {
        while (true)
        {
            //获取水平轴(Horizontal)
            float horizontal = Input.GetAxis("Horizontal");
            //获取垂直轴(Vertical)
            float vertical = Input.GetAxis("Vertical");


            if (axis_x_left && Input.GetKey(KeyCodeLeft))
            {
                // 计算水平方向的移动增量
                float horizontalIncrement = -0.1f * speed * Time.fixedDeltaTime;

                // 更新水平位置，同时限制速度
                if (Mathf.Abs(horizontal) < speed_max_x)
                {
                    horizontal += horizontalIncrement;
                }
                position_now_x += horizontal / 20;
            }
            else if (axis_x_right && Input.GetKey(KeyCodeRight))
            {
                // 计算水平方向的移动增量
                float horizontalIncrement = 0.1f * speed * Time.fixedDeltaTime;

                // 更新水平位置，同时限制速度
                if (Mathf.Abs(horizontal) < speed_max_x)
                {
                    horizontal += horizontalIncrement;
                }
                position_now_x += horizontal / 20;
            }

            if (axis_y_up && Input.GetKey(KeyCodeUp) && !isJumping)
            {
                // 计算垂直方向的移动增量
                float verticalIncrement = 0.1f * speed * Time.fixedDeltaTime;

                // 更新垂直位置，同时限制速度
                if (Mathf.Abs(vertical) < speed_max_y)
                {
                    vertical += verticalIncrement;
                }
                position_now_y += vertical / 20;
            }
            else if (axis_y_down && Input.GetKey(KeyCodeDown) && !isJumping)
            {
                // 计算垂直方向的移动增量
                float verticalIncrement = -0.1f * speed * Time.fixedDeltaTime;

                // 更新垂直位置，同时限制速度
                if (Mathf.Abs(vertical) < speed_max_y)
                {
                    vertical += verticalIncrement;
                }
                position_now_y += vertical / 20;
            }
/*             // 未拆分变量的代码段
            if ((Input.GetKey(KeyCodeLeft) || Input.GetKey(KeyCodeRight)) && axis_x)
            {
                // 计算水平方向的移动增量
                float horizontalIncrement = Input.GetKey(KeyCodeLeft) ? -0.1f * speed * Time.fixedDeltaTime : 0.1f * speed * Time.fixedDeltaTime;
                
                // 更新水平位置，同时限制速度
                if (Mathf.Abs(horizontal) < speed_max_x)
                {
                    horizontal += horizontalIncrement;
                }
                position_now_x += horizontal / 20;
            }

            if ((Input.GetKey(KeyCodeUp) || Input.GetKey(KeyCodeDown)) && axis_y)
            {
                // 计算垂直方向的移动增量
                float verticalIncrement = Input.GetKey(KeyCodeUp) ? 0.1f * speed * Time.fixedDeltaTime : -0.1f * speed * Time.fixedDeltaTime;
                
                // 更新垂直位置，同时限制速度
                if (Mathf.Abs(vertical) < speed_max_y)
                {
                    vertical += verticalIncrement;
                }
                position_now_y += vertical / 20;
            } */

            
            // 跳跃逻辑
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space)) && !isJumping  && axis_jump)
            {
                CharacterInteractionPositionTrigger.ReturnFloor = false;
                original_y = position_now_y;
                jumpForce_ = jumpForce;
                isJumping = true;
            }
            else if (isJumping  && axis_jump)
            {
                jumpForce_ -= gravity;
                position_now_y += jumpForce_;
                if (jumpForce_<0 && Mathf.Abs(jumpForce_) > jumpForce && jumpMode == true) //无地板，哪里起跳回到哪里
                {
                    isJumping = false;
                    position_now_y = original_y;
                }
                else if (CharacterInteractionPositionTrigger.ReturnFloor && jumpMode == false) //有地板，落到地板才停止
                {
                    isJumping = false;
                }
            }

            // 累积位置更新
            newPosition = new Vector3(position_now_x, position_now_y, position_now_z);

            // 最后应用累积的位置变化
            transform.position = newPosition;
/*             if(Input.GetKey(KeyCode.A))
            {
                if(Math.Abs(horizontal)<speed_max_x)
                {
                    horizontal += -0.00001f * speed;
                }    
                position_now_x += horizontal/20;
                transform.position = new Vector3(position_now_x,position_now_y,position_now_z);                    
            }
            else if(Input.GetKey(KeyCode.D))
            {
                if(Math.Abs(horizontal)<speed_max_x)
                {
                    horizontal += 0.00001f * speed;
                }   
                position_now_x += horizontal/20;
                transform.position = new Vector3(position_now_x,position_now_y,position_now_z);
            }

            if(Input.GetKey(KeyCode.W))
            {
                if(Math.Abs(vertical)<speed_max_y)
                {
                    vertical += 0.00001f * speed;
                }
                position_now_y += vertical/20;
                transform.position = new Vector3(position_now_x,position_now_y,position_now_z);    
            }
            else if(Input.GetKey(KeyCode.S))
            {
                if(Math.Abs(vertical)<speed_max_y)
                {
                    vertical += -0.00001f * speed;
                }
                position_now_y += vertical/20;
                transform.position = new Vector3(position_now_x,position_now_y,position_now_z);
            } */
            /* Debug.Log(transform.position); */
            yield return new WaitForFixedUpdate(); /* WaitForSeconds(0.01f); */
        }
    }

    void Start() //测试用
    {
        PositionSet(0,25,0);
        PositionMove("on");
    }
}
