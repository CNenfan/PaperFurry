using UnityEngine;
using System;

public class PlayerTeleporter : MonoBehaviour
{
    private Vector3 savedPosition = Vector3.zero; // 默认保存位置为原点

    private void Start()
    {
        // 尝试从PlayerPrefs读取保存的位置
        if (PlayerPrefs.HasKey("position"))
        {
            ReadPositionFromPrefs();
        }
    }

    private void Update()
    {
        // 检测R键是否被按下
        if (Input.GetKey(KeyCode.R))
        {
            ReadPositionFromPrefs();
        }

        // 检测玩家的y坐标是否小于-20
        if (transform.position.y < -20)
        {
            ReadPositionFromPrefs();
        }
    }

    private void ReadPositionFromPrefs()
    {
        string positionStr = PlayerPrefs.GetString("position");

        if (!string.IsNullOrEmpty(positionStr))
        {
            // 移除括号并分割字符串
            string[] parts = positionStr.Trim('(', ')').Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 3)
            {
                float x = float.Parse(parts[0]);
                float y = float.Parse(parts[1]);
                float z = float.Parse(parts[2]);

                savedPosition = new Vector3(x, y, z);
                transform.position = savedPosition;
            }
            else
            {
                Debug.LogError("Invalid position format.");
            }
        }
        else
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }
}