using UnityEngine;
using System.Collections;

/// <summary>
/// 控制2D游戏角色动画交互的控制器类。
/// </summary>
public class CharacterInteractionAnimationController : MonoBehaviour
{
    // 引用Animator组件，用于控制动画
    public Animator animator;

    // 引用SpriteRenderer组件，用于控制角色的方向
    public SpriteRenderer spriteRenderer;

    // 引用CharacterInteractionPosition组件，用于获取是否在跳跃
    public CharacterInteractionPosition characterInteractionPosition;

    // 引用粒子系统
    public ParticleSystem starParticles;

    [Space][Space]
    [SerializeField]
    private bool isEffects = true;

    // 记录角色的最后移动方向
    [SerializeField]
    private bool isFacingRight = true;
    public bool lastFacingRight = true;


    /// <summary>
    /// 在脚本实例化时调用，用于初始化Animator和SpriteRenderer组件的引用。
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterInteractionPosition = FindObjectOfType<CharacterInteractionPosition>();

        // 检查Animator组件是否存在
        if (animator == null)
        {
            Debug.LogError("未在当前游戏对象上找到Animator组件。");
        }

        // 检查SpriteRenderer组件是否存在
        if (spriteRenderer == null)
        {
            Debug.LogError("未在当前游戏对象上找到SpriteRenderer组件。");
        }
    }

    /// <summary>
    /// 每帧调用一次，用于更新动画状态和角色方向。
    /// </summary>
    
        // 添加一个翻转持续时间变量
        [SerializeField]
        private float flipDuration = 0.5f; // 完整翻转所需的时间



    private void Update()
    {
        // 获取水平方向的原始输入值
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 检查角色是否正在移动
        bool isWalking = Mathf.Abs(horizontalInput) > 0f;

        // 开启或关闭粒子系统
        if(isEffects){starParticles.gameObject.SetActive(isWalking);}

        // 只有当角色正在移动时才更新最后的移动方向
        if (isWalking)
        {
            isFacingRight = horizontalInput > 0f;
        }

        if (isFacingRight != lastFacingRight)
        {
            StartCoroutine(FlipCoroutine(isFacingRight));
        }

        // 记录最后的移动方向
        lastFacingRight = isFacingRight;

        // 更新Animator中的isWalking参数，控制动画状态
        animator.SetBool("isWalking", (isWalking && !characterInteractionPosition.isJumping));

        // 检查Animator组件是否有效，并验证isWalking参数是否正确设置
        if (animator != null)
        {
            bool isWalkingFromAnimator = animator.GetBool("isWalking");
            //Debug.Log($"Animator中的isWalking: {isWalkingFromAnimator}");
        }
        else
        {
            Debug.LogError("Animator组件无效, 无法验证参数。");
        }
    }

    private IEnumerator FlipCoroutine(bool newFacingDirection)
{
    float targetAngle = newFacingDirection ? 0f : 180f;
    Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
    float startTime = Time.time;

    while (true)
    {
        float elapsed = Time.time - startTime;
        float t = Mathf.Min(elapsed / flipDuration, 1f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
        

        if (t >= 1f)
        {
            yield break;
        }

        yield return null;
    }
}
}