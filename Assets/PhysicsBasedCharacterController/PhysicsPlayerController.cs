using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPlayerController : MonoBehaviour
{
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

    public GameObject TripShotPrefab;
    public GameObject TripShotGhostPrefab;
    public GameObject TripShotHandHeldPrefab;
    GameObject TripShotGhost = null;
    GameObject TripShot = null;
    TripShotTool TripShotScript;
    GameObject TripShotHandheld = null;

    [HideInInspector]
    public bool m_tripShotPlaced;
    [HideInInspector]
    public bool m_EnabledTripShot;
    [HideInInspector]
    public bool m_tripShotGhostExists;

    public float m_tripshotDistZ;
    public float m_tripshotDistX;
    public float m_tripshotDistY;
    public float m_tripshotRot;
    public float m_tripshotPlaceDist = 3;

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

    private void LateUpdate()
    {
       /* if (!m_tripShotPlaced)
        {
            if (!m_EnabledTripShot)
            {
                m_EnabledTripShot = true;
                TripShotHandheld = Instantiate(TripShotHandHeldPrefab, Camera.main.transform);
                TripShotHandheld.transform.localEulerAngles = new Vector3(TripShotHandheld.transform.localEulerAngles.x, TripShotHandheld.transform.localEulerAngles.y, TripShotHandheld.transform.localEulerAngles.z + m_tripshotRot);
                TripShotHandheld.transform.localPosition = new Vector3(TripShotHandheld.transform.localPosition.x + m_tripshotDistX, TripShotHandheld.transform.localPosition.y + m_tripshotDistY, TripShotHandheld.transform.localPosition.z + m_tripshotDistZ);
            }

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, m_tripshotPlaceDist))
            {
                if (hit.transform.gameObject.tag == "TripShotPlaceable")
                {
                    if (!m_tripShotGhostExists && Vector3.Distance(this.transform.position, hit.point) > 1.2f)
                    {
                        m_tripShotGhostExists = true;
                        TripShotGhost = Instantiate(TripShotGhostPrefab);
                        TripShotGhost.transform.position = hit.point;
                        TripShotGhost.transform.eulerAngles = Vector3.zero;
                    }
                    else if (Vector3.Distance(this.transform.position, hit.point) > 1.2f)
                    {
                        TripShotGhost.transform.position = hit.point;
                        TripShotGhost.transform.eulerAngles = Vector3.zero;
                    }

                    if (Input.GetMouseButtonDown(0) && TripShotGhost != null && TripShotHandheld != null)
                    {
                        TripShot = Instantiate(TripShotPrefab);
                        TripShotScript = TripShot.GetComponent<TripShotTool>();
                        TripShot.transform.parent = null;
                        TripShot.transform.position = hit.point;
                        Destroy(TripShotGhost);
                        Destroy(TripShotHandheld);
                        m_tripShotPlaced = true;
                        m_tripShotGhostExists = false;
                    }
                }
                else if (TripShotGhost != null && m_tripShotGhostExists)
                {
                    Destroy(TripShotGhost);
                    m_tripShotGhostExists = false;
                }
            }
            else if (TripShotGhost != null && m_tripShotGhostExists)
            {
                Destroy(TripShotGhost);
                m_tripShotGhostExists = false;
            }
        }
        else
        {
            RaycastHit hit;

            if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 2))
            {
                if (hit.transform.GetComponent<TripShotTool>() != null)
                {
                    if (TripShotScript._PlungerShot)
                    {
                        Destroy(TripShotScript._Plunger.GetComponent<LookAtVelocity>().TripShotRope);
                        TripShotScript._PlungerShot = false;
                        Destroy(TripShotScript._Plunger);
                    }

                    m_tripShotPlaced = false;
                    Destroy(TripShot);
                    m_EnabledTripShot = false;
                }
            }
        }*/
    }

    Vector3 calcMoveRot()
    {
        Vector3 moveRot = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveRot += Camera.main.transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveRot += -Camera.main.transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveRot += -Camera.main.transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveRot += Camera.main.transform.right;
        }
        if (moveRot == Vector3.zero)
        {
            moveRot = this.gameObject.transform.forward;
        }

        return moveRot;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "TripShotCord")
        {
            Destroy(TripShotScript._Plunger);

            TripShotScript._PlungerShot = false;
            if (TripShotScript._PlungerScript)
                Destroy(TripShotScript._PlungerScript.TripShotRope);
            TripShotScript._PlungerScript = null;
        }
        else if (collision.rigidbody)
        {
            collision.rigidbody.AddForceAtPosition(playerRigidbody.velocity * 200, collision.GetContact(0).point);
        }
    }
}
