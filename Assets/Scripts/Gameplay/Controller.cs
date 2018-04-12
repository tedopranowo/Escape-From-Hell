using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class Controller : MonoBehaviour
{
    private Character m_character;
    public Character character
    {
        set
        {
            m_character = value;
        }
        get
        {
            if (m_character == null)
                m_character = GetComponent<Character>();

            return m_character;
        }
    }
}
