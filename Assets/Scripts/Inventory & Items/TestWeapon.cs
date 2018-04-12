using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : MonoBehaviour {

    [SerializeField]
    Weapon m_weapon;
    // Use this for initialization

    public Weapon weapon
    {
        get { return m_weapon; }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   
}
