//GoToLocationStateData.cs
//Jakub Stopyra
//11/11/2017

using UnityEngine;

//interface that can be used for GoToLocation state data inharitance
//Interface used to get current target position.
public interface GoToLocationStateData{
    Vector3 GetCurrentTarget();
}
