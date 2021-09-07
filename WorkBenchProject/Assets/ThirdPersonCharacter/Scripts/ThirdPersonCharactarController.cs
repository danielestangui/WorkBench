using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class ThirdPersonCharactarController : MonoBehaviour
{

    private InputHandler inputHandler;

    private float moveAmount;
    public float moveSpeed = 3;
    public float runSpeed = 5;
    public float jumpForce = 10f;
    public float rotateSpeed = 5;
    private float distToGround = 0.1f;

    private bool isRunning;
    private bool isGrounded;
    private bool isJumping;

    private Vector3 moveDir;
    public Transform camTrans;

    private Animator anim;
    private Rigidbody rb;

    private float vertical;
    private float horizontal;

    [HideInInspector]
    public LayerMask ignoreLayer;

    private void Start()
    {
        inputHandler = gameObject.GetComponent<InputHandler>();
        anim = gameObject.GetComponentInChildren<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();

        rb.angularDrag = 999;
        rb.drag = 4;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        ignoreLayer = ~(1 << 8);
    }

    void FixedUpdate()
    {
        isGrounded = OnGorund();
        isRunning = inputHandler.runInput;
        isJumping = inputHandler.jumpInput;

        Move();
        Jump();
        Rotate();
        UpdateAnimator();
    }

    /// <summary>
    /// Movimiento del personaje
    /// </summary>
    private void Move()
    {
        horizontal = inputHandler.leftAxis.x;
        vertical = inputHandler.leftAxis.y;

        Vector3 v = vertical * camTrans.transform.forward;
        Vector3 h = horizontal * camTrans.transform.right;
        moveDir = (v + h).normalized;

        float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        moveAmount = Mathf.Clamp01(m);

        rb.drag = (moveAmount > 0 || isGrounded == false) ? 0 : 4;

        float targetSpeed = moveSpeed;

        if (isRunning)
            targetSpeed = runSpeed;

        if (isGrounded)
            rb.velocity = moveDir * (targetSpeed * moveAmount);

    }

    private void Jump() 
    {
        if (isJumping && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Rotacion del personaje
    /// </summary>
    private void Rotate() 
    {
        Vector3 targetDir = moveDir;
        targetDir.y = 0;

        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.fixedDeltaTime * moveAmount * rotateSpeed);
        transform.rotation = targetRotation;
    }

    /// <summary>
    /// Conrpueba que el jugador esta tocando el suelo
    /// </summary>
    /// <returns></returns>
    private bool OnGorund() 
    {
        bool r = false;

        Vector3 origin = transform.position + (Vector3.up * 0.3f);
        Vector3 dir = -Vector3.up;
        float dis = distToGround + 0.3f;
        RaycastHit hit;

        Color raycolor = Color.red;
        
        if (Physics.Raycast(origin, dir, out hit, dis, ignoreLayer)) 
        {
            raycolor = Color.green;
            r = true;
            //Vector3 targetPosition = hit.point;
            //transform.position = targetPosition;
        }

        Debug.DrawRay(origin, dir * dis, raycolor);

        return r;
    }

    /// <summary>
    /// Actualiza las variables del animator
    /// </summary>
    private void UpdateAnimator() 
    {
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetBool("IsJumping", isJumping);
        anim.SetBool("IsRunning", isRunning && (moveAmount >= 0.1));
        anim.SetFloat("Speed", moveAmount, 0.4f, Time.fixedDeltaTime);
    }
}
