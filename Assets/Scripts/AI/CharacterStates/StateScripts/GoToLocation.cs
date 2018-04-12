//GoToLocation.cs
//Jakub Stopyra
//11/11/2017

//state that tells the MovementComponent of the character to go to a specifit Vector3 position in the world
public class GoToLocation : CharacterState
{
//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //Called when a character with this state enters this state
    public override void OnEnter(Controller controller, CharacterStateData data)
    {
        base.OnEnter(controller, data);

    }

    //called when a character with this state gets ticked
    //TODO: Make the character check if the target has moved, if so then retarget our MovementComponent.
    public override void OnTick(Controller controller, CharacterStateData data)
    {
        base.OnTick(controller, data);
        //tell movement component of our character to go to our current target position.
        (controller as AiController).movement.GoToLocation((data as GoToLocationStateData).GetCurrentTarget());
    }

    //called when a character with this state exits this state
    public override void OnExit(Controller controller)
    {
        base.OnExit(controller);

        //stop the movement when we're done going to the location
        (controller as AiController).movement.StopMovement();
    }
}
