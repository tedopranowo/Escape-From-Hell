//CharacterState.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//This is a base CharacterState used to tell a character to perform specific actions.
public class CharacterState : ScriptableObject
{
    //---------------------------------------------------------------------------------------------------------------------
    //public API Methods
    //---------------------------------------------------------------------------------------------------------------------

    //called when the state is first entered by a character
    public virtual void OnEnter(Controller controller, CharacterStateData data)
    {
        //
    }

    //called when the state is being exit by a character
    public virtual void OnExit(Controller controller)
    {
        //
    }

    //called on tick of the character that owns that state
    public virtual void OnTick(Controller controller, CharacterStateData data)
    {
        //
    }

}
