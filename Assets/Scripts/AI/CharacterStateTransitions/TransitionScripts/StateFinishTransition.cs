using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* [Tedo]:
 * Personally, I don't think a state can ever be 'finished'.
 * However, a state can be invalid. For an example, if a unit is at the state of attacking an object,
 * when the target object is actually destroyed, that state will no longer be valid (You can't attack
 * object that no longer exist). It has similar implementation, but makes more sense to me. */
public class StateFinishTransition : CharacterStateTransition {

    public override bool ShouldTransition(Controller character, CharacterStateTransitionData data)
    {

        if ((data as StateFinishTransitionData).state.isFinished)
            return true;
        return false;
    }
}
