//CharacterStateTransition.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//CharacterStateTransition is a class that will determine if a state wants to transition to another state.
//CharacterStateTransition can store data on its specific CharacterStateTransitionData.
public class CharacterStateTransition : ScriptableObject
{
//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //called when the state is first entered by a character
    public virtual void OnEnter(Controller controller)
    {

    }

    //called on tick of the character that owns that state
    public virtual void OnTick(Controller controller)
    {

    }

    //called when the state is being exit by a character
    public virtual void OnExit(Controller controller)
    {

    }

    //called when we want to know if we should transition out of state
    public virtual bool ShouldTransition(Controller controller, CharacterStateTransitionData data)
    {
        return false;
    }
}
