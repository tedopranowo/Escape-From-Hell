//CharacterStateData.cs
//Jakub Stopyra
//11/11/2017

using System.Collections.Generic;
using UnityEngine;

public class CharacterMultiStateData : CharacterStateData
{
    //-----------------------------------------------------------------------------------------------------------------
    //Variables exposed to inspector
    //-----------------------------------------------------------------------------------------------------------------

    //state we will run. This has to be initialized in an inspector because CharacterState that we choose can differ from state to state.
    [SerializeField]
    [Tooltip("Character state to run")]
    List<CharacterStateData> m_states = new List<CharacterStateData>();

    public List<CharacterStateData> states { get { return m_states; } }

    int _currentState = 0;
    public int currentState { get { return _currentState; } set { _currentState = value; } }


    //-----------------------------------------------------------------------------------------------------------------
    //public API Methods
    //-----------------------------------------------------------------------------------------------------------------
    //called when a character enters a this state
    public override void OnEnter(Controller controller)
    {
        //enter every transition.
        for (int i = 0; i < m_transitions.Count; ++i)
        {
            m_transitions[i].transition.OnEnter(controller);
        }
        if (state)
            state.OnEnter(controller, this);
    }

    //called when character with this state gets ticked
    public override void OnTick(Controller controller)
    {
        if (state)
            state.OnTick(controller, this);
    }

    //called when a character with this state transitions to another state and exits this state
    public override void OnExit(Controller controller)
    {
        //reset our isFinished tag.
        isFinished = false;
        //exit our current state
        if (m_states.Count > 0)
        {
            for (int i = 0; i < m_states.Count; ++i)
            {
                m_states[i].OnExit(controller);
            }
        }
        //exit every transition.
        for (int i = 0; i < m_transitions.Count; ++i)
        {
            m_transitions[i].transition.OnExit(controller);
        }
    }

    public override void OnCreate(Controller controller)
    {
        base.OnCreate(controller);
        for (int i = 0; i < m_states.Count; ++i)
        {
            if (!m_states[i].beingCreated)
                m_states[i].OnCreate(controller);
        }
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        for (int i = 0; i < m_states.Count; ++i)
        {
            if (!m_states[i].beingDestroyed)
                m_states[i].OnDestroy();
        }
    }
}
