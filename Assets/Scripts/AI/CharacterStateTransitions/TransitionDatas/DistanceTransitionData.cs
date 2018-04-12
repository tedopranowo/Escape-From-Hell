//DistanceTransitionData.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//Transition data for distance transition.
//Data on this transition:
//target - target to check distance to.
//distance - distance at which we can transition.
public class DistanceTransitionData : CharacterStateTransitionData {
//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //target to which we are calculating distance towards
    [SerializeField][Tooltip("Target")]
    Transform m_target = null;

    //distance we need to meet
    [SerializeField][Tooltip("Distance")]
    float m_distance = 0;

//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //Get target
    public Transform target
    {
        get { return m_target; }
        set { m_target = value; }
    }

    //Get distance
    public float distance
    {
        get{ return m_distance; }
    }
}
