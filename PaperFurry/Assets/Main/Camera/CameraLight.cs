using UnityEngine;

public class CameraLight : MonoBehaviour
{
    public Light _light;

    private void Awake()
    {
        // 获取当前游戏对象上的Light组件
        _light = GetComponent<Light>();
    }

    // 定义打开灯光的函数LightOn
    public void LightOn()
    {
        if (_light != null)
        {
            _light.enabled = true;
            //Debug.Log("Light turned on.");
        }
        else
        {
            Debug.LogError("Light component is not found on this game object. Please ensure it is attached.");
        }
    }

    // 定义关闭灯光的函数LightOff
    public void LightOff()
    {
        if (_light != null)
        {
            _light.enabled = false;
            //Debug.Log("Light turned off.");
        }
        else
        {
            Debug.LogError("Light component is not found on this game object. Please ensure it is attached.");
        }
    }
}