//TimeStateTransition.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//Transition that is Valid after a given amount of seconds.
public class TimeStateTransition : CharacterStateTransition
{
//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //transition when the time specified is met
    public override bool ShouldTransition(Controller controller, CharacterStateTransitionData data)
    {
        //if our finishTime is more than game time then our transition is valid and we should transition.
        if ((data as TimeTransitionData).finishTime < Time.time)
        {
            return true;
        }
        return false;
    }
}
