using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Player : MonoBehaviour
{
    private Character m_character;

    public Character character
    {
        get { return m_character; }
        set { m_character = value; }
    }

    [SerializeField]
    private static Player s_instance = null;

    public static Player Instance()
    {

        if (!s_instance)
        {
            new GameObject("Player", typeof(Player));
        }
        return s_instance;
    }

    private void Awake()
    {
        s_instance = this;
        this.GetComponent<Rigidbody2D>().freezeRotation = true;
        DontDestroyOnLoad(gameObject);
    }
}
