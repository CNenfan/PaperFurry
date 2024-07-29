using UnityEngine;

public class SkyboxSwitcher : MonoBehaviour
{
    public Material[] skyboxes; // 在Inspector中添加多个天空盒材质

    private int currentSkyboxIndex = 0; // 当前选中的天空盒索引

    private void Update()
    {
        // 按下J键切换到下一个天空盒
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchToNextSkybox();
        }
        // 按下K键切换到上一个天空盒
        else if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchToPreviousSkybox();
        }
    }

    private void SwitchToNextSkybox()
    {
        // 循环到数组的下一个元素
        currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxes.Length;
        RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        // 强制Unity重新加载天空盒材质
        GL.InvalidateState();
    }

    private void SwitchToPreviousSkybox()
    {
        // 循环到数组的上一个元素
        currentSkyboxIndex = (currentSkyboxIndex - 1 + skyboxes.Length) % skyboxes.Length;
        RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        // 强制Unity重新加载天空盒材质
        GL.InvalidateState();
    }
}