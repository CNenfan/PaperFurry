using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PaperFurryBackgroundControl : MonoBehaviour
{
    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int windowSize = 20; // 动态加载的图片数量
    [SerializeField] private int minIndex = 1;
    [SerializeField] private int maxIndex = 281;
    [SerializeField] private int currentImageIndex = 180;
    private Dictionary<int, Sprite> loadedSprites = new Dictionary<int, Sprite>();

    [Space][Space][Header("Movement Settings")]
    [SerializeField] private float lastPositionX;
    [SerializeField] private float currentFrameLength = 0f;
    [SerializeField] private float frameLength = 0.08f; // 每帧长度的X坐标变化量
    [SerializeField] private bool isMovingRight = true; // 物体初始移动方向
    

    private void Start()
    {
        lastPositionX = transform.position.x;
        if(spriteRenderer == null)
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        LoadSprite(currentImageIndex);
    }

    private void Update()
    {
        float currentPositionX = transform.position.x;
        float deltaX = currentPositionX - lastPositionX;

        // 根据物体的移动方向更新累计的X坐标变化量
        if (deltaX > 0)
        {
            currentFrameLength += deltaX;
            isMovingRight = true;
        }
        else if (deltaX < 0)
        {
            currentFrameLength -= deltaX;
            isMovingRight = false;
        }

        // 当累计的X坐标变化量超过frameLength时，切换图片
        if (Mathf.Abs(currentFrameLength) >= frameLength)
        {
            if (isMovingRight)
            {
                // 物体向右移动，加载编号较大的图片
                if (currentImageIndex < maxIndex)
                {
                    if (!loadedSprites.ContainsKey(currentImageIndex + 1))
                    {
                        LoadSprite(currentImageIndex + 1);
                    }
                    currentImageIndex++;
                    spriteRenderer.sprite = loadedSprites[currentImageIndex];
                }
            }
            else
            {
                // 物体向左移动，加载编号较小的图片
                if (currentImageIndex > minIndex)
                {
                    if (!loadedSprites.ContainsKey(currentImageIndex - 1))
                    {
                        LoadSprite(currentImageIndex - 1);
                    }
                    currentImageIndex--;
                    spriteRenderer.sprite = loadedSprites[currentImageIndex];
                }
            }
            
            // 重置累计的X坐标变化量
            currentFrameLength = 0f;
        }

        // 更新上一位置记录
        lastPositionX = currentPositionX;
    }

    private void LoadSprite(int index)
    {
        if (!loadedSprites.ContainsKey(index))
        {
            string imageName = $"A-PaperFurryBg{index:D3}";
            Sprite sprite = Resources.Load<Sprite>(imageName);
            if (sprite == null)
            {
                Debug.LogError($"Failed to load sprite with name: {imageName}.jpg");
                return;
            }
            loadedSprites[index] = sprite;
        }
    }

    private void UnloadSprites(int startIndex)
    {
        for (int i = startIndex; i < startIndex + windowSize; i++)
        {
            if (loadedSprites.ContainsKey(i) && i != currentImageIndex)
            {
                loadedSprites.Remove(i);
            }
        }
    }
}