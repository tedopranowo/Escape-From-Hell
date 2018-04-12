using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//character multistate that will play all the provided states in sync
public class MultistateSynced : CharacterState {
    public override void OnEnter(Controller controller, CharacterStateData data)
    {
        //get our state data casted
        CharacterMultiStateData myData = data as CharacterMultiStateData;
        //enter the state.
        if (myData.states.Count > 0)
        {
            for (int i = 0; i < myData.states.Count; ++i)
            {
                myData.states[i].OnEnter(controller);
            }
        }
        base.OnEnter(controller, data);
    }

    public override void OnTick(Controller controller, CharacterStateData data)
    {
        //get our state data casted
        CharacterMultiStateData myData = data as CharacterMultiStateData;
        //tick our state
        if (myData.states.Count > 0)
        {
            for (int i = 0; i < myData.states.Count; ++i)
            {
                CharacterStateData newState = myData.states[i].GetStateOnTransition(controller);
                if (newState)
                {
                    myData.states[i].OnExit(controller);
                    myData.states[i] = newState;
                    newState.OnEnter(controller);
                }

                myData.states[i].OnTick(controller);
            }
        }
        base.OnTick(controller, data);
    }

    public override void OnExit(Controller controller)
    {
        base.OnExit(controller);
    }
}
