//CharacterStateTransitionData.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//CharacterStateTransitionData is a class that holds data for it's specified character transition.
public class CharacterStateTransitionData : MonoBehaviour
{
//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //our transition state we store data for.
    [SerializeField][Tooltip("Transition that we store data for.")]
    CharacterStateTransition m_transition = null;

//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //gets called when a character enters a state with this transition on it.
    public virtual void OnEnter(Controller controller)
    {
        //enter the transition state
        m_transition.OnEnter(controller);
    }

    //gets called when a character with this transition on it ticks.
    public virtual void OnTick(Controller controller)
    {
        //tick the transition state
        m_transition.OnTick(controller);
    }

    //gets called when a character exits a state with this transition on it.
    public virtual void OnExit(Controller controller)
    {
        //exit the transition state
        m_transition.OnExit(controller);
    }

    //do we want to transition.
    public bool ShouldTransition(Controller controller)
    {
        //ask the transition state if we want to transition
        return m_transition.ShouldTransition(controller, this);
    }
}
