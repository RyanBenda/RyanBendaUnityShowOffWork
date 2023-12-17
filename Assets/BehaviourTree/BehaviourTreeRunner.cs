using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    // Start is called before the first frame update
    void Start()
    {

        tree.gameObject = this.gameObject;
        tree.AssignGameObjectToNodes();

        tree = tree.Clone();

        

    }

    // Update is called once per frame
    void Update()
    {
        tree.Update();
    }
}
