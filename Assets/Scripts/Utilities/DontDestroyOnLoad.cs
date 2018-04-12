//DontDestroyOnLoad.cs
//Created by: Tedo Pranowo (tedokdr@yahoo.com)
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	// Use this for initialization
	private void Awake () {
        DontDestroyOnLoad(gameObject);
	}

}
