using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectApplier : MonoBehaviour {

    [SerializeField] Character m_targetCharacter = null;
    [SerializeField] StatusEffect m_statusEffect = null;
    [SerializeField] KeyCode m_keyButton = KeyCode.None;

    private void Update()
    {
        if (Input.GetKeyDown(m_keyButton))
        {
            m_targetCharacter.ApplyStatusEffect(m_statusEffect);
        }
    }

}
