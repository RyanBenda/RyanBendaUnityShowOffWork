using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    public static ToolManager instance;

    public HandHeldTripShot _HandHeldTripShot;
    public TripShotTool _CurTripShot;
    public HandHeldTrap _HandHeldTrap;
    public TrapTool _CurTrapTool;
    public GoggleTool _GoggleTool;
    public CameraTool _CurCameraTool;

    [HideInInspector]
    public List<Tool> _HandToolsList = new List<Tool>();

    public bool _HoldingTool;
    public Tool _CurrentTool;

    public MainCanvasComponent _MainCanvas;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        if (_MainCanvas == null)
        {
            _MainCanvas = FindObjectOfType<MainCanvasComponent>(true);
        }

        if (_HandHeldTripShot == null)
        {
            _HandHeldTripShot = FindObjectOfType<HandHeldTripShot>(true);
        }

        if (_HandHeldTrap == null)
        {
            _HandHeldTrap = FindObjectOfType<HandHeldTrap>(true);
        }

        if (_GoggleTool == null)
        {
            _GoggleTool = FindObjectOfType<GoggleTool>(true);
        }

        if (_CurCameraTool == null)
        {
            _CurCameraTool = FindObjectOfType<CameraTool>(true);
        }


        _HandToolsList.Add(_HandHeldTripShot);
        _HandToolsList.Add(_HandHeldTripShot);
        _HandToolsList.Add(_GoggleTool);
        _HandToolsList.Add(_CurCameraTool);

    }

    public void UnequipTools()
    {
        if (_CurTripShot != null)
            _CurTripShot.Unequip();
        if (_CurTrapTool != null)
            _CurTrapTool.Unequip();
        if (_CurCameraTool != null)
            _CurCameraTool.Unequip();
    }
}
