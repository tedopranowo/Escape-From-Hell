//IdleStateData.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//a state that makes the character rotate in space.
//this data contains:
//rotationSpeed
public class IdleStateData : CharacterStateData
{
    //[Jakub]TODO: Move this to an external file so its not owned by IdleStateData
    //Rotation Directions:
    public enum RotationDirections
    {
        left, right
    }

//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //speed at which the character will rotate
    [SerializeField][Tooltip("Rotation speed.")]
    float m_rotationSpeed = 0;

    //Max rotation in one direction
    [SerializeField][Tooltip("Max rotation in one direction.")]
    float m_maxRotation = 20;

    //Max number of rotations
    [SerializeField][Tooltip("Max number of rotations.")]
    int m_maxRotations = 1;

    //direction of rotation
    [SerializeField][Tooltip("Starting direction of rotation.")]
    RotationDirections m_startDirection = RotationDirections.left;

//-----------------------------------------------------------------------------------------------------------------
//Private variables
//-----------------------------------------------------------------------------------------------------------------
    [Tooltip("Our starting direction.")]
    Vector2 m_startedDir = Vector2.zero;
    Vector2 m_startedForward = Vector2.zero;

    float m_startedAngle = 0;

    //direction of rotation
    [Tooltip("current direction of rotation.")]
    RotationDirections m_direction = RotationDirections.left;
    
    //Max number of rotations
    [Tooltip("Current rotation.")]
    int m_currentRotation = 0;


//-----------------------------------------------------------------------------------------------------------------
//Private variable Getters/Setters
//-----------------------------------------------------------------------------------------------------------------
    //get direction
    public RotationDirections direction
    {
        get { return m_direction; }
    }

    //Get rotationSpeed
    public float rotationSpeed
    {
        get { return m_rotationSpeed; }
    }

//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    public override void OnEnter(Controller controller)
    {
        //save our starting direction
        m_startedDir = controller.transform.up;
        m_startedForward = controller.transform.forward;
        m_startedAngle = controller.transform.rotation.z;
        //reset the current rotation counter
        m_currentRotation = 0;
        //set our starting rotation
        m_direction = m_startDirection;
        //calls the state's OnEnter
        base.OnEnter(controller);
    }

    public override void OnTick(Controller controller)
    {
        //check how far have we rotated.
        float currentAngle = Vector2.Angle(m_startedDir, controller.transform.up);
        if (currentAngle > m_maxRotation)
        {
            /*[Tedo]:
             * can be refactored */
            #region refactor target
            //swap directions
            if (m_direction == m_startDirection)
            {
                if (m_direction == RotationDirections.left)
                    m_direction = RotationDirections.right;
                else
                    m_direction = RotationDirections.left;
            }
            else
            {
                if (m_direction == RotationDirections.left)
                    m_direction = RotationDirections.right;
                else
                    m_direction = RotationDirections.left;

                /*[Tedo]:
                 * Misleading name, maybe change it the name to m_currentCycle instead? */
                ++m_currentRotation;
            }
            #endregion
        }
        else if (currentAngle <= 0.5f)
        {
            /*[Tedo]:
             * I don't think a state should be able to be 'finished' (Read 'StateFinishTransition.cs').
             * I would limit the idle duration using TimeTransition instead of defining it in this state. 
             * However, you may keep this method if you prefer it*/
            if (m_direction == m_startDirection && m_currentRotation >= m_maxRotations)
            {
                isFinished = true;
            }
        }
        //ticks the state
        base.OnTick(controller);
    }

    public override void OnExit(Controller controller)
    {
        Debug.Log("Resetting rotation");
        Vector3 currentRotation = controller.transform.rotation.eulerAngles;
        currentRotation.z= m_startedAngle;
        controller.transform.rotation = Quaternion.Euler( currentRotation);
        base.OnExit(controller);
    }
}
