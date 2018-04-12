//IdleState.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//a state that makes the character rotate in space.
public class IdleState : CharacterState
{
//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //Called when a character with this state enters this state
    public override void OnTick(Controller character, CharacterStateData data)
    {
        base.OnTick(character, data);
        //check which direction we want to turn
        //if it's left then rotate on forward axis, if its right then rotate on -Forward axis.
        int directionSwap = (data as IdleStateData).direction == IdleStateData.RotationDirections.left ? 1 : -1;
        //asks the state data for the rotation speed, and apply that rotation to our character's transform rotation
        character.transform.Rotate(directionSwap*Vector3.forward, (data as IdleStateData).rotationSpeed*Time.deltaTime);
    }

}
