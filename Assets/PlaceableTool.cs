using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableTool : Tool
{
    [HideInInspector]
    public bool _ToolPlaced;
    [HideInInspector]
    public bool _ToolGhostExists;

    public float _ToolPlaceDist;

    [HideInInspector]
    public GameObject _ToolGhost;

    public GameObject _ToolPrefab;
    public GameObject _ToolGhostPrefab;

    public List<GameObject> _ToolOutlines = new List<GameObject>();
    public float _IncreasePlaceHeight = 0.025f;

    public virtual void PlaceTool() {}
    public virtual void PickUpTool() {}
    public virtual void ReturnTool() {}

}
