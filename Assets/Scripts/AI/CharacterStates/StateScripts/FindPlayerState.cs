using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlayerState : FindTargetState
{

    override public Transform FindTarget()
    {
        PlayerController player = PlayerController.instance;
        if (player)
            return player.transform;
        return null;
    }
}
