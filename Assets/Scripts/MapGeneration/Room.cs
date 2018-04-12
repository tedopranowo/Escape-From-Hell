using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    #region fields=================================================================================
    [SerializeField] private Transform[] m_itemSpawnLocation;
    [SerializeField]
    [Tooltip("If this is true, all door will be locked when the object is enabled.\n" +
        "If this is false, do nothing")]
    private bool m_lockAllDoor;
    #endregion

    #region properties=============================================================================
    public Vector3[] spawnPosition
    {
        get
        {
            return Array.ConvertAll(m_itemSpawnLocation, transform => transform.position);
        }
    }

    public bool lockAllDoor
    {
        set
        {
            m_lockAllDoor = value;

            //Change the door state
            Door[] doorList = GetComponentsInChildren<Door>();
            foreach (Door door in doorList)
            {
                door.IsLocked = value;
            }
        }
    }
    #endregion

    #region MonoBehaviours=================================================================================
    private void OnEnable()
    {
        //Lock all door if it is true
        if (m_lockAllDoor == true)
            lockAllDoor = true;
    }
    #endregion
}
