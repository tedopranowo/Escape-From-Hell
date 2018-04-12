using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CSEvent : CustomizableScript {
    public virtual bool ShouldTriggerOnApplied()
    {
        return false;
    }

    public virtual bool ShouldTriggerNow()
    {
        return false;
    }

    public virtual bool ShouldTriggerOnRemoved()
    {
        return false;
    }

    public virtual void Save(CustomSerializedData serializedData)
    {
    }

    public virtual void Load(string[] serializedData, int startIndex)
    {
    }
}

//-------------------------------------------------------------------------------------------------
// Type of CSEvent
//-------------------------------------------------------------------------------------------------

public class CSOnAppliedEvent : CSEvent
{
    public override bool ShouldTriggerOnApplied()
    {
        return true;
    }
    public override string ToString()
    {
        return "When buff is applied";
    }
}

[System.Serializable]
public class CSOnIntervalEvent : CSEvent
{
    [SerializeField] private float m_interval;
    private float m_nextTrigger;

    public override void OnApplied()
    {
        m_nextTrigger = Time.time + m_interval;
    }
    public override bool ShouldTriggerNow()
    {
        if (m_nextTrigger < Time.time)
        {
            m_nextTrigger = m_nextTrigger + m_interval;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return "Every " + m_interval + " seconds";
    }

    public override void Save(CustomSerializedData serializedData)
    {
        serializedData.AddFloat(m_interval);
    }

    public override void Load(string[] serializedData, int startIndex)
    {
        m_interval = float.Parse(serializedData[startIndex]);
    }
}

public class CSOnRemovedEvent : CSEvent
{
    public override bool ShouldTriggerOnRemoved()
    {
        return true;
    }
    public override string ToString()
    {
        return "When buff is removed";
    }
}