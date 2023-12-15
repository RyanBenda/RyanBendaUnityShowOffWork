using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class NodeCategoryAttribute : System.Attribute
{
    private string category;

    public NodeCategoryAttribute(string category)
    {
        this.category = category;
    }

    public string Category => category;
}

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }
    [HideInInspector] public string folderName = "";
    [HideInInspector] public GameObject attachedGameObject;
    [HideInInspector] public State state = State.Running;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;

    public State Update()
    {
        if(!started)
        {
            OnStart();
            started = true;
        }
        state = OnUpdate();


        //If it finished
        if (state == State.Failure || state ==  State.Success)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual Node Clone() {

        return Instantiate(this);
    }
    public virtual void AttachGameObject(GameObject gameObj)
    {
        attachedGameObject = gameObj;
    }
    protected abstract void OnStart();
 
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
