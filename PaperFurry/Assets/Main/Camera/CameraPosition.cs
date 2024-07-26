using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 摄像机位置设置

/// 一、直接控制摄像机位置（一次性）
/// CameraPositionSet(x,y,z);
/// 二、丝滑移动摄像机位置（一次性）
/// CameraPositionMove(mode,time,x,y,z);
/// 三、丝滑+自动计算中点摄像机位置（一次性）
/// CameraPositionMiddleCount(mode,time,x,y,z);
/// 四、持续移动摄像机位置至该点（持续的）
/// 参数设置:
/// mode:on/off/position1/position2/speed
/// CameraPositionMoveContinue("on"/"off");
/// CameraPositionMoveContinue("position1"/"position2",x,y,z);
/// CameraPositionMoveContinue("speed",speed);

/// </summary>


[System.Serializable]   //可被序列化
public class CameraPosition : MonoBehaviour
{
    public void CameraPositionSet(float x,float y = 0,float z = 0)
    {
        transform.position = new Vector3(x,y,z);
    }


    private Vector3 targetPosition;
    /* private float currentTime; */
    private float duration = 0.5f; // Default duration
    private Coroutine currentMoveCoroutine;
    public void CameraPositionMove(string mode = "Absolute",float time = 5f,float x = 0,float y = 0,float z = -10,float maxSpeedMultiplier = 1f)
    {
        duration = time;
        switch (mode)
        {
            case "Absolute":    //绝对路径
                {
                    targetPosition = new Vector3(x, y, z);
                    /* currentTime = 0f; */
                    break;
                }
            case "Relative":    //相对路径
                {
                    targetPosition = transform.position + new Vector3(x, y, z);
                    /* currentTime = 0f; */
                    break;
                }
            default:
                {
                    Debug.LogError("CameraPositionMove()函数的mode参数错误");
                    break;
                }
        }
        if (currentMoveCoroutine != null)
        {
            StopCoroutine(currentMoveCoroutine);
        }
        Vector3 startPosition = transform.position;
        Vector3 endPosition = targetPosition;        
        currentMoveCoroutine = StartCoroutine(SmoothMoveCameraCoroutine(startPosition, endPosition, duration, maxSpeedMultiplier));
    }

    //更新的代码，从CharacterMessageBoxControl.cs Move函数下复制
    public IEnumerator SmoothMoveCameraCoroutine(Vector3 startPosition, Vector3 endPosition, float time, float maxSpeedMultiplier = 1f)
    {
        bool isFinished = false;
        float timer = 0f;

        // 加速和减速阶段各为time秒
        float accelerationDuration = time;
        float decelerationDuration = time;

        // 匀速运动时间根据起始位置、目标位置和总时间自动计算
        float constantSpeedDuration = Mathf.Max(0, time * 2 - Vector2.Distance(startPosition, endPosition) / maxSpeedMultiplier);

        while (!isFinished && timer <= time * 2)
        {
            float t;

            // 加速阶段
            if (timer <= accelerationDuration)
            {
                t = Mathf.SmoothStep(0f, 1f, timer / accelerationDuration);
            }
            // 匀速阶段
            else if (timer <= accelerationDuration + constantSpeedDuration)
            {
                t = Mathf.Clamp01((timer - accelerationDuration) / constantSpeedDuration);
            }
            // 减速阶段
            else
            {
                t = 1f - Mathf.SmoothStep(0f, 1f, (timer - accelerationDuration - constantSpeedDuration) / decelerationDuration);
            }

            transform.localPosition = Vector2.Lerp(startPosition, endPosition, t);
            timer += Time.deltaTime;

            // 判断是否已经到达目标位置（这里假设允许小范围内的误差）
            if (Vector2.Distance(transform.localPosition, endPosition) < 0.001f)
            {
                isFinished = true;
            }
            yield return null; // Wait for next frame
        }
        // 取整修正
        transform.localPosition = endPosition;
        yield return null; // Wait for next frame
    }

/*  //无法准确控制时间而删除的代码
    private IEnumerator SmoothMoveCameraCoroutine()
    {
        while (transform.position != targetPosition)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / duration);
            transform.position = Vector3.Lerp(transform.position, targetPosition, t);

            yield return null; // Wait for next frame
        }

        //取整修正
        transform.position = targetPosition;
        currentMoveCoroutine = null;
    } */


    //自动计算到两个定位点的中点
    public void CameraPositionMiddleCount(string mode = "Absolute",float time = 0.5f,float x = 0,float y = 0,float z = -10)
    {
        x = (transform.position.x + x) / 2;
        y = (transform.position.y + y) / 2;
        z = (transform.position.z + z) / 2;
        CameraPositionMove(mode,time,x,y,z);
    }


    /// 持续移动摄像机位置至该点（持续的）
    /// 参数设置:
    /// mode:on/off/position1/position2/speed
    /// CameraPositionMoveContinue("on"/"off");
    /// CameraPositionMoveContinue("position1"/"position2",x,y,z);
    /// CameraPositionMoveContinue("speed",speed);
    
    public static Vector3 position1,position2;
    public static float speed = 1f;
    public void CameraPositionMoveContinue(string mode, float x_or_speed = 3f, float y = 0, float z = -10)
    {
        switch (mode)
        {
            case "position1":
                // 记录第一个位置
                position1 = new Vector3(x_or_speed, y, z);
                break;
            case "position2":
                // 记录第二个位置
                position2 = new Vector3(x_or_speed, y, z);
                break;
            case "on":
                // 使用新的速度参数重新调用 CameraPositionMove
                if (currentMoveCoroutine != null)
                {
                    StopCoroutine(currentMoveCoroutine);
                }
                // 启动协程
                currentMoveCoroutine = StartCoroutine(CameraPositionMoveContinueIEnumerator());
                break;
            case "off":
                // 停止协程
                if (currentMoveCoroutine != null)
                {
                    StopCoroutine(currentMoveCoroutine);
                    currentMoveCoroutine = null;
                }
                break;
            case "speed":
                // 更新速度
                speed = 0.5f / x_or_speed; // 根据速度调整默认持续时间
                break;
            default:
                Debug.LogError("CameraPositionMoveContinue()函数的mode参数错误");
                break;
        }
    }

    private IEnumerator CameraPositionMoveContinueIEnumerator(float duration = 0f)
    {
        while (true)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = Vector3.Lerp(position1, position2, 0.5f); // 示例：计算两个位置的中点作为目标位置

            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                
                // KEY: 控制更新间隔
                yield return new WaitForSeconds(0.1f);
                
                timeElapsed += Time.deltaTime;
            }

            // 完全到达目标位置
            transform.position = endPosition;
            
            // 可选：在移动完成后等待一段时间再开始下一次移动
            yield return null;
        }
    }
/*     private IEnumerator CameraPositionMoveContinueIEnumerator()
    {
        while (true)
        {
            Vector3 startPosition = position1;
            Vector3 endPosition = position2;
            float duration = speed;

            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            // 等待一段时间再开始下一次移动
            yield return new WaitForSeconds(0.0001f);
        }
    } */
}
