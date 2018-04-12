using UnityEngine;

[System.Serializable]
public class CSEventWrapper : CSEvent
{
    //---------------------------------------------------------------------------------------------
    // Enum
    //---------------------------------------------------------------------------------------------
    public enum Type
    {
        kNone,
        kOnApplied,
        kInterval,
        kOnRemoved
    }

    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    private CSEvent m_csEvent;
    private Type m_type;
    [SerializeField] private CustomSerializedData m_serializedData = null;

    //---------------------------------------------------------------------------------------------
    // Constructor
    //---------------------------------------------------------------------------------------------
    public CSEventWrapper(CSEventWrapper copyFrom)
    {
        //We don't need to copy m_csEvent since it will be loaded in the script using
        //m_serializedData as the source
        m_type = copyFrom.m_type;
        m_serializedData = copyFrom.m_serializedData;
    }

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    public CSEvent csEvent
    {
        set
        {
            m_csEvent = value;
        }
        get
        {
            return m_csEvent;
        }
    }
    public Type type
    {
        set
        {
            System.Type targetType = null;
            switch(value)
            {
                case Type.kOnApplied:
                    targetType = typeof(CSOnAppliedEvent);
                    break;
                case Type.kInterval:
                    targetType = typeof(CSOnIntervalEvent);
                    break;
                case Type.kOnRemoved:
                    targetType = typeof(CSOnRemovedEvent);
                    break;
                default:
                    break;
            }

            if (m_csEvent != null && targetType == m_csEvent.GetType() || value == Type.kNone)
                return;

            m_type = value;
            m_csEvent = System.Activator.CreateInstance(targetType) as CSEvent;
        }
        get
        {
            return m_type;
        }
    }

    //---------------------------------------------------------------------------------------------
    // Functions
    //---------------------------------------------------------------------------------------------
    public override void OnApplied()
    {
        Load();
        m_csEvent.OnApplied();
    }

    public override void OnRemoved()
    {
        m_csEvent.OnRemoved();
    }

    public override bool ShouldTriggerOnApplied()
    {
        return m_csEvent.ShouldTriggerOnApplied();
    }

    public override bool ShouldTriggerNow()
    {
        return m_csEvent.ShouldTriggerNow();
    }

    public override bool ShouldTriggerOnRemoved()
    {
        return m_csEvent.ShouldTriggerOnRemoved();
    }
     
    public void Save()
    {
        m_serializedData.Reset();

        //Save csEvent type
        m_serializedData.AddInt((int) m_type);

        //Save the event data
        csEvent.Save(m_serializedData);
    }

    public void Load()
    {
        if (!m_serializedData.IsEmpty())
        {
            //Get the data
            string[] stringList = m_serializedData.GetData();

            //Create the event type
            type = (Type)int.Parse(stringList[0]);

            //Load the data for the event
            m_csEvent.Load(stringList, 1);
        }
    }

    public override string ToString()
    {
        return (m_csEvent != null) ? m_csEvent.ToString() : "Event";
    }
}