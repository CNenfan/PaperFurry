using UnityEngine;
using System.Collections;

public class PaperFurryAnimation : MonoBehaviour
{
    public Animator animator;
    //public ParticleSystem starParticles;

    //[SerializeField]
    //private bool isEffects = true;

    private void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animator component not found.");
        }
    }

    // 记录角色的最后移动方向
    [SerializeField]
    private bool isFacingRight = true;
    public bool lastFacingRight = true;

    private void Update()
    {
        // 获取水平方向的原始输入值
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 检查角色是否正在移动
        bool isWalking = (Mathf.Abs(horizontalInput) > 0f) || (Mathf.Abs(verticalInput) > 0f);
        animator.SetBool("isWalking", (isWalking && !PaperFurryPosition3D.isJumping));

        // 开启或关闭粒子系统
        //if(isEffects){starParticles.gameObject.SetActive(isWalking);}

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
    }


        [SerializeField]
        private float flipDuration = 0.5f; // 完整翻转所需的时间

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