//WanderAroundStateData.cs
//Jakub Stopyra
//11/11/2017

using System.Collections.Generic;
using UnityEngine;

//Wander around state chooses a target at random from a list of targets.
public class WanderAroundStateData : CharacterStateData,  GoToLocationStateData {
    enum LoopType
    {
        random,
        loop,
        pingPong
    }

//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //targets we will be randomly walking to.
    [SerializeField][Tooltip("target transform")]
    List<Transform> m_targets = null;

    [SerializeField] [Tooltip("loop type between targets")]
    LoopType m_loopType = LoopType.loop;

    //-----------------------------------------------------------------------------------------------------------------
    //Private variables
    //-----------------------------------------------------------------------------------------------------------------
    //we need to tell a distance transition about our target.
    [Tooltip("Distance Transition data that checks distance to our target.")]
    DistanceTransitionData m_transitionData = null;

    //our current target index.
    int m_currentTarget = 0;
    //direction of the pingPong LoopType
    int m_pingPongDirection = 1;

    //-----------------------------------------------------------------------------------------------------------------
    //public API Methods
    //-----------------------------------------------------------------------------------------------------------------
    //called when a character with this state enters this state
    public override void OnEnter(Controller controller)
    {
        //find the distance transition data.
        FindTransitionData();

        //make sure we have targets.
        if (m_targets.Count <= 0)
            return;
        
        switch (m_loopType)
        {
            case LoopType.loop:
                //increment the target
                ++m_currentTarget;
                if (m_currentTarget >= m_targets.Count)
                    m_currentTarget = 0;
                break;
            case LoopType.pingPong:
                //increment or decrement the pingPong
                m_currentTarget += m_pingPongDirection;
                if (m_currentTarget >= m_targets.Count || m_currentTarget < 0)
                {
                    m_pingPongDirection *= -1;
                    m_currentTarget += m_pingPongDirection;
                    m_currentTarget += m_pingPongDirection;
                }
                break;
            case LoopType.random:
                //generate a new random target
                m_currentTarget = Random.Range(0, m_targets.Count);
                break;
        }

        //make sure the distance transition data exists
        if (m_transitionData)
            //set the current target as the target for distance transition.
            m_transitionData.target = m_targets[m_currentTarget];

        //call base
        base.OnEnter(controller);
    }

    //returns the current target from a list of targets under the currentTarget index
    public Vector3 GetCurrentTarget()
    {
        return m_targets[m_currentTarget].position;
    }

    public override void OnCreate(Controller controller)
    {
        DetachWaypoints(controller);
        if (beingCreated)
            return;
        base.OnCreate(controller);
    }

    public override void OnDestroy()
    {
        if (beingDestroyed)
            return;
        base.OnDestroy();
        DestroyWaypoints();
    }

    //-----------------------------------------------------------------------------------------------------------------
    //private internal Methods
    //-----------------------------------------------------------------------------------------------------------------
    private void FindTransitionData()
    {
        for (int i = 0; i < m_transitions.Count; ++i)
        {
            if (m_transitions[i].transition is DistanceTransitionData)
            {
                m_transitionData = m_transitions[i].transition as DistanceTransitionData;
            }
        }
    }

    //offset our waypoints to the middle of the room
    private void DetachWaypoints(Controller controller)
    {
        //make sure we have any targets.
        if (m_targets.Count == 0)
        {
            return;
        }

        //find out what is the room we are in.
        //find the camera manager
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        Transform roomTransform = null;
        //make sure that the camera manager was found
        if (cameraManager)
        {
            roomTransform = cameraManager.targetRoom.transform;
        }

        //find the middle of the room.
        //make sure we have found the room we were looking for
        Vector2 centerOfRoom = Vector2.zero;
        if (roomTransform)
        {
            centerOfRoom = roomTransform.transform.position;
        }

        //move the waypoint holder to the middle of the room
        //make sure we have found a valid location
        if(centerOfRoom != Vector2.zero)
        {
            m_targets[0].parent.position = centerOfRoom;
        }

        //unparent my waypoints
        m_targets[0].parent.SetParent(controller.transform.parent);
    }

    private void DestroyWaypoints()
    {
        //make sure we have any targets.
        if (m_targets.Count == 0)
        {
            return;
        }
        Debug.Log("Destroying waypoints");
        //make sure the object we are destorying has not bee ndestoryed by Unity yet.
        if(m_targets[0] != null)
            Destroy(m_targets[0].parent.gameObject);
    }
}
