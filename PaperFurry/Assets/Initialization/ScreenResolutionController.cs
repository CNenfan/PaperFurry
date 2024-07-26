#if !UNITY_EDITOR
using UnityEngine;

public class ScreenResolutionController
{
    // 常用的2D游戏分辨率预设
    public enum PresetResolutions
    {
        Resolution_1280x720,
        Resolution_1600x900,
        Resolution_1920x1080,
        Resolution_3840x2160
    }

    //非常早的初始化
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void ScreenResolutionInit()
    {
        // 获取并打印当前屏幕分辨率
        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;
        Debug.Log($"当前屏幕分辨率为: {currentWidth} x {currentHeight}");

        if (!PlayerPrefs.HasKey("OriginalScreenWidth") || !PlayerPrefs.HasKey("OriginalScreenHeight"))
        {
            PlayerPrefs.SetInt("OriginalScreenWidth", Screen.currentResolution.width);
            PlayerPrefs.SetInt("OriginalScreenHeight", Screen.currentResolution.height);
        }
        
        /*// 示例：临时修改分辨率
        SetResolutionToPreset(PresetResolutions.Resolution_1920x1080); */
    }

    void SetResolutionToPreset(PresetResolutions preset)
    {
        switch (preset)
        {
            case PresetResolutions.Resolution_1280x720:
                Screen.SetResolution(1280, 720, true);
                break;
            case PresetResolutions.Resolution_1600x900:
                Screen.SetResolution(1600, 900, true);
                break;
            case PresetResolutions.Resolution_1920x1080:
                Screen.SetResolution(1920, 1080, true);
                break;
            case PresetResolutions.Resolution_3840x2160:
                Screen.SetResolution(3840,2160,true);
                break;
            default:
                Debug.LogWarning("未识别的分辨率预设");
                break;
        }
    }

    public void QuitGame()
    {
        // 尝试恢复原始分辨率
        if (PlayerPrefs.HasKey("OriginalScreenWidth") && PlayerPrefs.HasKey("OriginalScreenHeight"))
        {
            int originalWidth = PlayerPrefs.GetInt("OriginalScreenWidth");
            int originalHeight = PlayerPrefs.GetInt("OriginalScreenHeight");
            Screen.SetResolution(originalWidth, originalHeight, true); // 最后一个参数为是否全屏
        }

        Application.Quit(); // 退出应用
    }
}
#endif