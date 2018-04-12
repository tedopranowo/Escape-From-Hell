//StateFinishTransitionData.cs
//Jakub Stopyra
//11/16/2017

using UnityEngine;

//Transition data for state finish check.
//Data on this transition:
//state - state we are checking if its finished.
public class StateFinishTransitionData : CharacterStateTransitionData
{
    //-----------------------------------------------------------------------------------------------------------------
    //Variables exposed to inspector
    //-----------------------------------------------------------------------------------------------------------------
    //state that we are checking if its finished
    [SerializeField]
    [Tooltip("State")]
    CharacterStateData m_state = null;

    //-----------------------------------------------------------------------------------------------------------------
    //public API Methods
    //-----------------------------------------------------------------------------------------------------------------
    //Get/Set state
    public CharacterStateData state
    {
        get { return m_state; }
        set { m_state = value; }
    }
}
