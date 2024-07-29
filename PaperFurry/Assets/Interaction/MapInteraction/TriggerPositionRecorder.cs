using UnityEngine;

public class TriggerPositionRecorder : MonoBehaviour
{
    // 用于记录触发事件时的位置
    private Vector3 triggerPosition;
    private bool hasTriggered = false; // 标记触发器是否已被触发

    // 改变触发器的网格渲染器引用，以便改变颜色
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.red;

    }

    private void OnTriggerEnter(Collider other)
    {
        // 确保只执行一次
        if (!hasTriggered)
        {
            triggerPosition = transform.position;
            PlayerPrefs.SetString("position", $"({triggerPosition.x}, {triggerPosition.y+3}, {triggerPosition.z})");
            PlayerPrefs.Save();
            hasTriggered = true;

            // 改变颜色为绿色
            meshRenderer.material.color = Color.green;
        }
    }
}
