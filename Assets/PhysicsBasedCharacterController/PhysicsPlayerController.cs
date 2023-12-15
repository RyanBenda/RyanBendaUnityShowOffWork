using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsPlayerController : MonoBehaviour
{
    public PlayerInput _PlayerInput;

    public float m_MoveSpeed;

    bool m_NewMovement;

    //public float m_InterpolationSpeed;

    public bool m_CanJump = false;
    public float m_JumpForce = 5;
    public bool m_Grounded = false;
    public Vector3 m_GroundedCheckHalfExtents;
    public float m_GroundedCheckDist;
    public float m_MaxJumps = 1;
    float m_jumpTally = 0;
    bool m_prevFrameJumpCheck;

    public float m_CustomDrag = 0.1f;
    public float m_LessAirDrag;

    public bool canSprint;
    public float m_SprintSpeedMultiplier = 2;

    Rigidbody playerRigidbody;

    public bool _HoldingTool;
    public Tool _CurrentTool;
    public PlayerStates _PlayerState;
    public ToolManager _ToolManager;
    public bool moving;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        m_Grounded = GroundedCheck();

        if (!m_Grounded)
        {
            m_prevFrameJumpCheck = true;
        }
        if (m_prevFrameJumpCheck && m_Grounded)
        {
            m_jumpTally = 0;
            m_prevFrameJumpCheck = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_CanJump)
        {
            if (m_Grounded || m_jumpTally < m_MaxJumps)
            {
                m_jumpTally++;
                playerRigidbody.AddForce(new Vector3(playerRigidbody.velocity.x, m_JumpForce, playerRigidbody.velocity.z));
            }
        }

        Camera.main.transform.parent.transform.position = this.transform.position;
    }

    void FixedUpdate()
    {
        Vector3 moveDir = calcMoveDir();
        if (moveDir.x != 0 || moveDir.z != 0)
            m_NewMovement = true;

        Vector3 camUpNormalized = Camera.main.transform.forward;
        camUpNormalized.y = 0.0f;
        camUpNormalized.Normalize();

        Vector3 camRightNormalized = Camera.main.transform.right;
        camRightNormalized.y = 0.0f;
        camRightNormalized.Normalize();

        Vector3 cameraForwardCopy = Vector3.zero;
        cameraForwardCopy += camUpNormalized * moveDir.z;
        cameraForwardCopy += camRightNormalized * moveDir.x;

        Vector3 force = cameraForwardCopy.normalized * (m_MoveSpeed * 1000000) * Time.deltaTime;

        if (canSprint && Input.GetKey(KeyCode.LeftShift))
            force *= m_SprintSpeedMultiplier;

        bool m_isMovement = false;

        if (m_NewMovement)
        {
            playerRigidbody.AddForce(force);
            m_NewMovement = false;
            m_isMovement = true;
        }

        Vector3 curVel = playerRigidbody.velocity;
        curVel.y = 0.0f;
        if (m_Grounded || m_isMovement)
        {
            playerRigidbody.AddForce(-curVel * m_CustomDrag);
            m_isMovement = false;
        }
        else
            playerRigidbody.AddForce((-curVel * m_CustomDrag) / m_LessAirDrag);
    }

    Vector3 calcMoveDir()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = Vector3.ClampMagnitude(new Vector3(x, 0, z), 1);

        return moveDir;
    }

    bool GroundedCheck()
    {
        bool isGrounded = false;
        RaycastHit hit;

        if (Physics.BoxCast(this.transform.position, m_GroundedCheckHalfExtents, -Vector3.up, out hit, Quaternion.identity, m_GroundedCheckDist))
        {
            if (playerRigidbody.velocity.y == 0)
                isGrounded = true;
        }
        else if (m_jumpTally == 0)
            m_jumpTally++;

        return isGrounded;
    }
}
