using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clink3D 类继承自 MonoBehaviour，用于处理游戏中的三维点击事件。
/// 它通过监控移动设备上的触摸事件来模拟鼠标点击操作。
/// </summary>
public class Clink3D : MonoBehaviour
{
    /// <summary>
    /// Update 方法由 Unity 每帧调用。
    /// 用于处理输入触摸事件，并向场景中投射射线以检测是否有对象被点击。
    /// </summary>
    void Update()
    {
        RaycastHit hit; // 声明一个 RaycastHit 变量来存储射线碰撞信息。
        
        // 遍历所有当前的触摸点。
        for (int i = 0; i < Input.touchCount; ++i)
        {
            // 检查当前触摸事件是否刚刚开始。
            if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
            {
                // 根据当前触摸坐标构建射线。
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                
                // 使用物理引擎进行射线检测，如果射线击中了场景中的物体，则将击中信息存入 hit 变量。
                if (Physics.Raycast(ray, out hit))
                {
                    // 向被击中的游戏对象发送 OnMouseDown 消息，模拟鼠标按下事件。
                    hit.transform.gameObject.SendMessage("OnMouseDown");
                }
            }
        }
    }
}