using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCursorComponent : MonoBehaviour
{
    public PhysicsPlayerController _Player;
    public GameObject _cursorImage;

    // Start is called before the first frame update
    void Awake()
    {
        if (_Player == null)
            _Player = Camera.main.transform.root.GetComponent<PhysicsPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_Player._PlayerState == PlayerStates.WeaponWheelState || _Player._PlayerState == PlayerStates.InventoryState || _Player._PlayerState == PlayerStates.PauseState)
        {
            this._cursorImage.SetActive(true);

            this.transform.position = Input.mousePosition;
        }
        else
        {
            this._cursorImage.SetActive(false);
            this.transform.GetComponent<RectTransform>().position = new Vector3(9999, 9999, 9999);
        }
    }
}
