//StraightMovementComponent.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//movement component that makes the character move to its target in a straight line
public class StraightMovementComponent : MovementComponent {

//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //Called when a state or a character wants to go to a location.
    //Go to a location specified in a straight line.
    public override void GoToLocation(Vector3 location)
    {
        //calculate our movement direction
        Vector2 direction = location - m_rigidbody.transform.position;
        //set velocity of our controlled rigidbody to the direction normalized * the speed of the movement
        m_rigidbody.velocity = direction.normalized * m_speed;
    }

    
}
