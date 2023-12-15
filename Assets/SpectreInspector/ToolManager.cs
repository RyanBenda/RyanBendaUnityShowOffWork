using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public TripShotTool _CurTripShot;
    public TrapTool _CurTrapTool;
    public TrackerTool _CurTrackerTool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnequipTools()
    {
        if (_CurTripShot != null)
            _CurTripShot.Unequip();
        if (_CurTrapTool != null)
            _CurTrapTool.Unequip();
        if (_CurTrackerTool != null)
            _CurTrackerTool.Unequip();
    }
}
