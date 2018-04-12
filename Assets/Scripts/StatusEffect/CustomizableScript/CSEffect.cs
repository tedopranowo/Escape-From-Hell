using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CSEffect {
    [SerializeField] private CSEventWrapper m_event = null;
    [SerializeField] private CSActionWrapper m_action = null;

    //---------------------------------------------------------------------------------------------
    // Constructor
    //---------------------------------------------------------------------------------------------
    public CSEffect(CSEffect copyFrom)
    {
        m_event = new CSEventWrapper(copyFrom.m_event);
        m_action = new CSActionWrapper(copyFrom.m_action);
    }

    public void OnApplied(Character owner)
    {
        m_event.OnApplied();
        m_action.OnApplied();

        //Check if the action should be applied
        if (m_event.ShouldTriggerOnApplied())
            m_action.ApplyAction(owner);
    }

    public void OnTick(Character owner)
    {
        //Check if the action should be applied
        if (m_event.ShouldTriggerNow())
            m_action.ApplyAction(owner);
    }

    public void OnRemoved(Character owner)
    {
        m_event.OnRemoved();
        m_action.OnRemoved();

        //Check if the action should be applied
        if (m_event.ShouldTriggerOnRemoved())
            m_action.ApplyAction(owner);
    }
}