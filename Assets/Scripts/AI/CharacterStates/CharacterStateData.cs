//CharacterStateData.cs
//Jakub Stopyra
//11/11/2017

using System.Collections.Generic;
using UnityEngine;

//data carrying transition and a state that transition is for.
[System.Serializable]
public class TransitionData
{
//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //disable warning saying that we're never using this variable. We are never using this variable, it's only for inspector purposes.
#pragma warning disable 0414
    //the only reason for this variable is to display a name of the transition in the inspector.
    [SerializeField][Tooltip("Name of transition")]
    string m_name = "transition";
#pragma warning restore 0414

    //class used to determine if we want to transition here
    [SerializeField][Tooltip("Transition data")]
    CharacterStateTransitionData m_transition = null;

    //state we will transition to
    [SerializeField][Tooltip("State to transition to")]
    CharacterStateData m_state = null;

//-----------------------------------------------------------------------------------------------------------------
//Private variable Getters/Setters
//-----------------------------------------------------------------------------------------------------------------
    //get the transition
    public CharacterStateTransitionData transition
    {
        get { return m_transition; }
    }

    //get the state
    public CharacterStateData state
    {
        get { return m_state; }
    }
}

public class CharacterStateData : MonoBehaviour {
//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //list of transitions from this state
    [SerializeField][Tooltip("Transitions to other states")]
    protected List<TransitionData> m_transitions = new List<TransitionData>();

    //state we will run. This has to be initialized in an inspector because CharacterState that we choose can differ from state to state.
    [SerializeField][Tooltip("Character state to run")]
    CharacterState m_state = null;
    public CharacterState state { get { return m_state; } }

    //-----------------------------------------------------------------------------------------------------------------
    //Private variables
    //-----------------------------------------------------------------------------------------------------------------
    [Tooltip("Is the state finished.")]
    bool m_isFinished = false;

    //are we being destroyed
    bool m_beingDestroyed = false;
    public bool beingDestroyed { get { return m_beingDestroyed; } }

    //are we being created
    bool m_beingCreated = false;
    public bool beingCreated { get { return m_beingCreated; } }

    //-----------------------------------------------------------------------------------------------------------------
    //Private variable Getters/Setters
    //-----------------------------------------------------------------------------------------------------------------
    //get is finished
    public bool isFinished
    {
        get { return m_isFinished; }
        protected set { m_isFinished = value; }
    }

//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //called when a character enters a this state
    public virtual void OnEnter(Controller controller)
    {
        //enter the state.
        if(m_state)
            m_state.OnEnter(controller, this);

        //enter every transition.
        for (int i = 0; i < m_transitions.Count; ++i)
        {
            m_transitions[i].transition.OnEnter(controller);
        }
    }

    //called when character with this state gets ticked
    public virtual void OnTick(Controller controller)
    {
        //tick our state
        if(m_state)
            m_state.OnTick(controller, this);
    }

    //called when a character with this state transitions to another state and exits this state
    public virtual void OnExit(Controller controller)
    {
        //reset our isFinished tag.
        m_isFinished = false;
        //exit our current state
        if(m_state)
            m_state.OnExit(controller);
        //exit every transition.
        for (int i = 0; i < m_transitions.Count; ++i)
        {
            m_transitions[i].transition.OnExit(controller);
        }
    }

    //When the object is created
    public virtual void OnCreate(Controller controller)
    {
        if (!m_beingCreated)
        {
            m_beingCreated = true;
            for (int i = 0; i < m_transitions.Count; ++i)
            {
                if(!m_transitions[i].state.beingCreated)
                m_transitions[i].state.OnCreate(controller);
            }
        }
        //m_beingCreated = false;
    }
    //When the object is destroyed
    public virtual void OnDestroy()
    {
        if (!m_beingDestroyed)
        {
            m_beingDestroyed = true;
            for (int i = 0; i < m_transitions.Count; ++i)
            {
                if(!m_transitions[i].state.beingDestroyed)
                    m_transitions[i].state.OnDestroy();
            }
        }
        //m_beingDestroyed = false;
    }

    //check if any of the state transitions are valid, if so then return the state that transition points to.
    public CharacterStateData GetStateOnTransition(Controller controller)
    {
        //for each transition that we have, check if we want to transition, if so then return the assosiated state.
        for (int i = 0; i < m_transitions.Count; ++i)
        {
            if(m_transitions[i].transition.ShouldTransition(controller))
            {
                return m_transitions[i].state;
            }
        }
        return null;
    }
}
