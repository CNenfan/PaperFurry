using UnityEngine;

public class CameraLoadUIOnScreen : MonoBehaviour
{
    [SerializeField] private GameObject cameraDebugDataPrefab; // 通过Inspector设置的预制体

    // 未来允许DataSystem操控

    private void Start()
    {
        CameraDebugData();
    }
    
    private void CameraDebugData()
    {
        // 加载预制体
        GameObject cameraDebugDataInstance = Instantiate(cameraDebugDataPrefab);

        // 获取CameraDebugData组件并调用ApplyDebugData方法
        CameraDebugData debugDataComponent = cameraDebugDataInstance.GetComponent<CameraDebugData>();
        debugDataComponent.ApplyDebugData(Camera.main);
    }

    
}