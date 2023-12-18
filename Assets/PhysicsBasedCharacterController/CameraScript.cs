using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public PhysicsPlayerController m_Player;
    public Transform m_CameraDefaultLocation;

    public float m_MouseXSensitivity = 1;
    public float m_MouseYSensitivity = 1;

    public float m_MaxCameraHeightClamp = 90;
    public float m_MinCameraHeightClamp = -90;

    public bool m_InvertCamY = false;

    public float m_SphereCastRadius = 0.1f;
    public float m_SphereCastRadius2 = 0.49f;
    public float minDistFromPlayer = 3;

    public float m_FacingUpMinimum = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        m_Player = FindObjectOfType<PhysicsPlayerController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (m_Player._PlayerState == PlayerStates.PlayState)
            RotateCamera();
    }

    void RotateCamera()
    {
        float xRot = this.gameObject.transform.eulerAngles.x;

        float tempxRot;
        if (m_InvertCamY)
            tempxRot = this.gameObject.transform.eulerAngles.x + Input.GetAxis("Mouse Y") * m_MouseYSensitivity;
        else
            tempxRot = this.gameObject.transform.eulerAngles.x - Input.GetAxis("Mouse Y") * m_MouseYSensitivity;

        if (this.gameObject.transform.eulerAngles.x > m_MaxCameraHeightClamp && this.gameObject.transform.eulerAngles.x < 180)
            xRot = m_MaxCameraHeightClamp;
        else if (this.gameObject.transform.eulerAngles.x < m_MinCameraHeightClamp + 360 && this.gameObject.transform.eulerAngles.x > 180)
            xRot = m_MinCameraHeightClamp;
        else if (Input.GetAxis("Mouse Y") > 0)
        {
            if (tempxRot < m_MaxCameraHeightClamp && tempxRot < 180)
                xRot = tempxRot;
            else if (tempxRot > m_MinCameraHeightClamp + 360 && tempxRot > 180)
                xRot = tempxRot;
        }
        else if (Input.GetAxis("Mouse Y") < 0)
        {
            if (tempxRot > m_MinCameraHeightClamp + 360 && tempxRot > 180)
                xRot = tempxRot;
            else if (tempxRot < m_MaxCameraHeightClamp && tempxRot < 180)
                xRot = tempxRot;
        }

        this.gameObject.transform.eulerAngles = new Vector3(xRot, this.gameObject.transform.eulerAngles.y, this.gameObject.transform.eulerAngles.z);
        m_Player.transform.eulerAngles = new Vector3(m_Player.transform.eulerAngles.x, m_Player.transform.eulerAngles.y + Input.GetAxis("Mouse X") * m_MouseXSensitivity, m_Player.transform.eulerAngles.z);
    }
}
