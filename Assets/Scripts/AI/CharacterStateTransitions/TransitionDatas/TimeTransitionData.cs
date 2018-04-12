//TimeTransitionData.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//data used for time transition
public class TimeTransitionData : CharacterStateTransitionData
{
//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //Duration that we will wait for before being valid.
    [SerializeField][Tooltip("Duration of our wait.")]
    float m_duration = 0;

//-----------------------------------------------------------------------------------------------------------------
//Private variables
//-----------------------------------------------------------------------------------------------------------------
    //This variable is not used yet, but is created in case it's needed in the future for more complex time keeping.
    float m_startTime = 0;

    //Time of the game at which the transition is valid.
    float m_finishTime = 0;

//-----------------------------------------------------------------------------------------------------------------
//Private variable Getters/Setters
//-----------------------------------------------------------------------------------------------------------------
    //Get finishTime
    public float finishTime
    {
        get { return m_finishTime; }
    }

//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //called when a character enters a state that owns this transition
    public override void OnEnter(Controller controller)
    {
        base.OnEnter(controller);
        //calculate at which time our transition is valid.
        m_startTime = Time.time;
        m_finishTime = m_startTime + m_duration;
    }
}
