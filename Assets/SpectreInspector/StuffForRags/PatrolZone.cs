using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolZone : MonoBehaviour
{
    public GameObject[] pathPoints = new GameObject[4]; // Can be as large as you need.
    public bool useAsOrderedChildren = false;
    public CreatureScriptableObject[] whoCanUsePath;


    // Start is called before the first frame update
    void Start()
    {


        if (useAsOrderedChildren)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                pathPoints[i] = transform.GetChild(i).gameObject;
            }
       
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
