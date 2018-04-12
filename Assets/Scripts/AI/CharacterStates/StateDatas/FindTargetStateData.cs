using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetStateData : CharacterStateData {

    //the target we have found
    Transform _target = null;
    public Transform target { get { return _target; } set { _target = value; } }

}
