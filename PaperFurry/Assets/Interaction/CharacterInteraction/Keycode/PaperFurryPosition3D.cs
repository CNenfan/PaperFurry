using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperFurryPosition3D : MonoBehaviour
{
    public static float position_now_x, position_now_y, position_now_z;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 0.1f;
    [SerializeField] private float rayLength = 1.1f; // 调整为比角色半高略大的值

    private Rigidbody rb;
    
    [SerializeField][ReadOnly]
    private bool isGrounded;
    private float lastJumpTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 减小阻力
        rb.drag = 0.01f;
    }

    public static bool isJumping = false;

    [SerializeField] private LayerMask groundLayer; // 添加地面层掩码

    private void Update()
    {
        // 检测地面接触
        isGrounded = Physics.Raycast(transform.position + Vector3.down * 0.5f, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer); // 调整起点和使用LayerMask

        // WADSD 移动
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized * moveSpeed;
        rb.AddForce(movement * Time.deltaTime, ForceMode.VelocityChange);

        // 空格键跳跃
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && Time.time - lastJumpTime > jumpCooldown)
        {
            Debug.Log("Grounded and Space pressed, attempting to jump.");
            isJumping = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            lastJumpTime = Time.time;
        }
        else if (isGrounded)
        {
            isJumping = false;
        }
    }

    private void FixedUpdate()
    {
        // 如果在地面上并且没有跳跃，则重置垂直速度
        if (isGrounded && !isJumping)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        }
        // 更新当前位置
        position_now_x = transform.position.x;
        position_now_y = transform.position.y;
        position_now_z = transform.position.z;
    }
}