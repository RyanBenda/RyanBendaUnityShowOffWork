using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToNewRoom : ActionNode
{

    BearRoom[] _BearRooms;
    GameObject[] _PotentialTPLocs;
    //Vector3 _currentRoom;

    CreatureBrain _brain;

    protected override void OnStart()
    {
        if (_brain == null)
            _brain = attachedGameObject.GetComponent<CreatureBrain>();

        if (_BearRooms == null)
        {
            _BearRooms = FindObjectsOfType<BearRoom>();
        }


        if (_PotentialTPLocs == null)
        {
            int amountOfRooms = 0;


            amountOfRooms = _BearRooms.Length;



            _PotentialTPLocs = new GameObject[amountOfRooms];
            int posCounter = 0;

           
            foreach (BearRoom room in _BearRooms)
            {
                _PotentialTPLocs[posCounter] = room.gameObject;
                posCounter++;
                //_PotentialTPLocs[posCounter] = door._Back;
            }
            
            
        }

        pickRoom();
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }

    void pickRoom()
    {
        int index = Random.Range(0, _PotentialTPLocs.Length);


        while (_BearRooms[index]._RoomInUse == true)
            index = Random.Range(0, _PotentialTPLocs.Length);
    
        



        GameObject GO = _PotentialTPLocs[index];
        if (GO != null)
        {
            attachedGameObject.transform.position = GO.transform.position;
            

            
            foreach (BearRoom room in _BearRooms)
            {
                room._RoomInUse = false;
            }
            _BearRooms[index]._RoomInUse = true;
            _brain._CurRoom = _BearRooms[index];
            
            
            

        }
        

    }
}
