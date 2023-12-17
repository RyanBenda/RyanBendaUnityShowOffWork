using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class PlungerComponent : MonoBehaviour
{
    public TripShotTool TripShot;

    bool canCollide;

    public bool m_NotInPlace = true;

    [SerializeField] private GameObject TripShotRopePrefab;
    [HideInInspector]
    public GameObject TripShotRope;

    [SerializeField] private GameObject ElectocutePrefab;

    float InitialTime;

    public Transform m_PlungerTail;

    float interpolateAmount;

    [HideInInspector]
    public Vector3 startPoint;
    [HideInInspector]
    public Vector3 midPoint;
    [HideInInspector]
    public Vector3 endPoint;
    [HideInInspector]
    public Quaternion finalRotation;

    float _DistEffect;
    [SerializeField] private float _DistEffectDivider = 10;

    [SerializeField] private float _PlungerFlightTime = 0.1f; //In Seconds

    [SerializeField] private AudioClip[] _Stuck;
    [SerializeField] private AudioSource _source;

    float interpolationEndValue = 1;

    [SerializeField] private Animator _PlungerAnimator;

    void Awake()
    {
        TripShot = FindObjectOfType<TripShotTool>();

        TripShotRope = Instantiate(TripShotRopePrefab);
        TripShotRope.transform.parent = null;

        TripShotRope.transform.GetChild(0).localScale = new Vector3(TripShotRope.transform.GetChild(0).localScale.x, (Vector3.Distance(m_PlungerTail.position, TripShot._BarrelBase.position) / 2), TripShotRope.transform.GetChild(0).localScale.z);
        TripShotRope.transform.position = TripShot._BarrelBase.position + ((m_PlungerTail.position - TripShot._BarrelBase.position) / 2);
        TripShotRope.transform.LookAt(m_PlungerTail);

        InitialTime = Time.time;

        startPoint = TripShot._BarrelLaunchPoint.position;
        midPoint = TripShot._PlungerFlightPathMiddlePoint;
        endPoint = TripShot._PlungerFinalPosTransform.position;

        _DistEffect = Vector3.Distance(startPoint, endPoint);
    }

    void Update()
    {
        // Moves the plunger along the path while making it rotate to face the direction its flying
        if (m_NotInPlace)
        {
            interpolateAmount = (interpolateAmount + (Time.deltaTime / (_PlungerFlightTime + (_DistEffect / _DistEffectDivider))));

            this.transform.position = TripShot.MovePlunger(startPoint, midPoint, endPoint, interpolateAmount);

            Vector3 newPos = TripShot.MovePlunger(startPoint, midPoint, endPoint, interpolateAmount);
            Vector3 newPos1 = TripShot.MovePlunger(startPoint, midPoint, endPoint, interpolateAmount + (Time.deltaTime / _PlungerFlightTime));      

            Vector3 temp = newPos1 - newPos;

            this.transform.forward = temp;

            if (interpolateAmount >= interpolationEndValue)
            {
                m_NotInPlace = false;
                _PlungerAnimator.SetBool("Landed", true);

                AudioClip audioClip = _Stuck[Random.Range(0, _Stuck.Length - 1)];
                _source.clip = audioClip;
                _source.Play();

                this.transform.position = TripShot._PlungerFinalPos;
                this.transform.rotation = Quaternion.LookRotation(-TripShot._PlungerFinalNormal);
            }
        }
        TripShotRope.transform.GetChild(0).localScale = new Vector3(TripShotRope.transform.GetChild(0).localScale.x, (Vector3.Distance(m_PlungerTail.position, TripShot._BarrelBase.position) / 2), TripShotRope.transform.GetChild(0).localScale.z);

        TripShotRope.transform.position = TripShot._BarrelBase.position + ((m_PlungerTail.position - TripShot._BarrelBase.position) / 2);
        TripShotRope.transform.LookAt(m_PlungerTail);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "TripShot" && canCollide)
        {
            m_NotInPlace = false;

            RaycastHit hit;

            Vector3 origin = this.transform.position;

            if (Physics.Raycast(origin, collision.GetContact(0).point - this.transform.position, out hit, 1000))
            {
                this.transform.position = collision.GetContact(0).point;

                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().isKinematic = true;

                if (TripShot._PlungerFinalNormalBool && Vector3.Distance(this.transform.position, TripShot._PlungerFinalPos) < 0.75f)
                {
                    this.transform.transform.rotation = Quaternion.LookRotation(-TripShot._PlungerFinalNormal);
                    TripShot._PlungerFinalNormalBool = false; 
                }
                else
                    this.transform.transform.rotation = Quaternion.LookRotation(-hit.normal);
            }
            if (Vector3.Distance(this.transform.position, TripShot._PlungerFinalPos) < 0.75f)
                this.transform.position = TripShot._PlungerFinalPos;
        }
    }

    public void SpawnElectrocute(Vector3 pos)
    {
        GameObject electro = Instantiate(ElectocutePrefab);
        electro.transform.position = pos;
    }
}
