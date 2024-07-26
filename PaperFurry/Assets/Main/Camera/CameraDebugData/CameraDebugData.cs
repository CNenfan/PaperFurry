using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraDebugData : MonoBehaviour
{
    public Camera mainCamera; // 主摄像机
    public Vector3 displacement; //相对于相机的位置偏移量



    public void ApplyDebugData(Camera camera)
    {
        this.mainCamera = camera;
        // 启动协程以定期更新UI文本
        StartCoroutine(FpsDataUpdate());

        //显示uid
        UidUpdate(UID);
    }
    private void UpdateUIPositionAndRotation()
    {
        /* Debug.Log(mainCamera.transform.position); */
        transform.position = mainCamera.transform.position + displacement;
    }

    /// <summary>
    /// fps显示部分
    /// </summary>
    [Space]
    public Text fpsText; // UI文本组件，用于显示FPS
    private float accumulatedTime = 0f; // 累计时间
    public float fpsUpdateTime = 0.3f; // 更新间隔，单位：秒
    private float nextUpdateTime = 0.0f; // 下次更新的时间
    private int frameCount = 0; // 帧计数
    float fps;
    private IEnumerator FpsDataUpdate()
    {
        //默认关闭FPS显示
        fpsText.gameObject.SetActive(false);
        
        //顺便操控一下其他的文字组件
        uidText.gameObject.SetActive(false);
        
        while (true)
        {
            // 更新累计时间和帧计数
            accumulatedTime += Time.deltaTime;
            frameCount++;

            // 更新UI元素的位置和旋转
            UpdateUIPositionAndRotation();

            // 检查是否到了更新时间
            if (Time.time >= nextUpdateTime)
            {
                // 计算平均FPS
                fps = frameCount / accumulatedTime;

                // 更新UI文本
                fpsText.text = $"FPS: {fps:F2}";

                // 重置累计时间和帧计数
                accumulatedTime = 0.0f;
                frameCount = 0;

                // 设置下次更新的时间
                nextUpdateTime = Time.time + fpsUpdateTime;
            }

            //检查F3是否启用文字
            if (Input.GetKey(KeyCode.F3))
            {
                fpsText.gameObject.SetActive(!fpsText.gameObject.activeSelf);

                //顺便操控一下其他的文字组件
                uidText.gameObject.SetActive(!uidText.gameObject.activeSelf);
                yield return new WaitForSeconds(0.1f);
            }
            yield return null;
        }
    }

    /// <summary>
    /// UID显示部分
    /// </summary>
    [Space]
    public Text uidText;
    public int UID = -1;
    public void UidUpdate(int uid = -1)
    {
        uidText.text = $"               UID: {UID}";
    }


}