using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : Controller {
//-----------------------------------------------------------------------------------------------------------------
//Variables exposed to inspector
//-----------------------------------------------------------------------------------------------------------------
    //movement component of this character
    [SerializeField]
    [Tooltip("Nav Mesh Agent used to drive this character")]
    protected MovementComponent m_movement = null;

    //initial state of our character.
    [SerializeField]
    [Tooltip("Initial state of the character.")]
    protected CharacterStateData m_initialState = null;


//-----------------------------------------------------------------------------------------------------------------
//Private variables
//-----------------------------------------------------------------------------------------------------------------
    //Our state tree
    [Tooltip("Current state of the character.")]
    protected CharacterStateData m_currState = null;

//-----------------------------------------------------------------------------------------------------------------
//Private variable Getters/Setters
//-----------------------------------------------------------------------------------------------------------------
    //Get movement component
    public MovementComponent movement
    {
        get { return m_movement; }
    }

//-----------------------------------------------------------------------------------------------------------------
//MonoBehavior Methods
//-----------------------------------------------------------------------------------------------------------------
    // Use this for initialization
    void Start()
    {
        Initialize();
    }

    public void Update()
    {
        HandleStates();
    }

    //when we get destroyed
    private void OnDestroy()
    {
        if (m_currState)
            m_currState.OnDestroy();
    }

//-----------------------------------------------------------------------------------------------------------------
//private internal Methods
//-----------------------------------------------------------------------------------------------------------------
    //Initialize neccesary variables
    private void Initialize()
    {
        //check if we have assigned a nav agent
        if (!movement)
            m_movement = GetComponent<MovementComponent>();

        //sets and runs our initial state as our current state.
        SetState(m_initialState);
        if (m_currState)
            m_currState.OnCreate(this);
    }

    //checks if we need to transition to a new state, ticks the current state
    private void HandleStates()
    {
        //check if we have a current state
        if (m_currState)
        {
            //Try getting a new state
            CharacterStateData newState = m_currState.GetStateOnTransition(this);   //[TODO]

            //if we have a new state from transition then set it.
            if (newState)
            {
                SetState(newState);
            }
        }

        //if we have a current state then tick it.
        if (m_currState)
            m_currState.OnTick(this);   //[TODO]
    }

    //enters a new state and exists the current state.
    private void SetState(CharacterStateData state)
    {
        //check if our current state is vald
        if (m_currState)
        {
            //exit the state
            m_currState.OnExit(this);   //[TODO]
        }

        //Check the new state is valid
        if (state)
            //enter the state
            state.OnEnter(this);        //[TODO]

        //set our current state to the new state
        m_currState = state;
    }
}
