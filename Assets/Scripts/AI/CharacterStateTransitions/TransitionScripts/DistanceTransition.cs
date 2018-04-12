//DistanceTransition.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//transition that is valid when a distance specified is met to a target specified.
public class DistanceTransition : CharacterStateTransition{
//-----------------------------------------------------------------------------------------------------------------
//public API Methods
//-----------------------------------------------------------------------------------------------------------------
    //transition if the distance to our target meets the distance specified
    public override bool ShouldTransition(Controller character, CharacterStateTransitionData data)
    {
        //check our position delta.
        Vector2 delta = (data as DistanceTransitionData).target.position - character.transform.position;

        //get the distance squared that we need to meet.
        float distanceSquared = (data as DistanceTransitionData).distance.Squared();
        
        //check if the distance squared to target is less than the distance squared that we need to meet.
        if (delta.sqrMagnitude <= distanceSquared)
        {
            return true;
        }
        return false;
    }
}
