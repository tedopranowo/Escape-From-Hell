//EnemyCharacter.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//component responsible to dictate the movement of a character
public abstract class MovementComponent : MonoBehaviour {
//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //speed used to mvoe the character
    [SerializeField][Tooltip("Base speed of the movement")]
    protected float m_speed = 0.0f;

    //rigidbody that moves the character around the map
    [SerializeField][Tooltip("Rigidbody 2d of our character")]
    protected Rigidbody2D m_rigidbody = null;

//-----------------------------------------------------------------------------------------------------------------
//Private variable Getters/Setters
//-----------------------------------------------------------------------------------------------------------------

//-----------------------------------------------------------------------------------------------------------------
//MonoBehavior Methods
//-----------------------------------------------------------------------------------------------------------------
   
//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //Set the go to location position.
    public abstract void GoToLocation(Vector3 location);

    //stop the movement of the character
    public void StopMovement()
    {
        m_rigidbody.velocity = Vector2.zero;
    }

//-----------------------------------------------------------------------------------------------------------------
//private internal Methods
//-----------------------------------------------------------------------------------------------------------------
    //Initialize neccesary variables
    private void Initialize()
    {
        //check if we know whats our rigidbody to controll
        if (!m_rigidbody)
            m_rigidbody = GetComponent<Rigidbody2D>();
    }
}
