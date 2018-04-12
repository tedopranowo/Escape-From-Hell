using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : Interactable
{
    private bool m_isChangingScene = false;

    public override void Interact()
    {
        if (m_isChangingScene == false)
        {
            Destroy(gameObject);
            SceneManagerSingleton.instance.LoadNextLevel();
            m_isChangingScene = true;
        }
    }
}
