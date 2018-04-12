using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : CustomButton {

   

    public override void OnClick()
    {
        SceneManagerSingleton.instance.LoadNextLevel();
    }

}
