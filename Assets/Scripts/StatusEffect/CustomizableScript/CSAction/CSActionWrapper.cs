using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CSActionWrapper : CSAction {
    //---------------------------------------------------------------------------------------------
    // Enum
    //---------------------------------------------------------------------------------------------
    public enum Type
    {
        kNone,
        kDealDamage,
        kHeal,
        kModifyMovementSpeed
    }

    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    private CSAction m_csAction;
    private Type m_type = Type.kNone;
    [SerializeField] private CustomSerializedData m_serializedData = null;

    //---------------------------------------------------------------------------------------------
    // Constructor
    //---------------------------------------------------------------------------------------------
    public CSActionWrapper(CSActionWrapper copyFrom)
    {
        m_type = copyFrom.m_type;
        m_serializedData = copyFrom.m_serializedData;
    }

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    public CSAction csAction
    {
        set
        {
            m_csAction = value;
        }
        get
        {
            return m_csAction;
        }
    }
    public Type type
    {
        set
        {
            System.Type targetType = null;
            switch(value)
            {
                case Type.kDealDamage:
                    targetType = typeof(CSActionDealDamage);
                    break;
                case Type.kHeal:
                    targetType = typeof(CSActionHeal);
                    break;
                case Type.kModifyMovementSpeed:
                    targetType = typeof(CSActionModifyMovementSpeed);
                    break;
            }

            if (m_csAction != null && targetType == m_csAction.GetType() || value == Type.kNone)
                return;

            m_type = value;
            m_csAction = System.Activator.CreateInstance(targetType) as CSAction;
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
    }

    public override void ApplyAction(Character target)
    {
        m_csAction.ApplyAction(target);
    }

    public void Save()
    {
        m_serializedData.Reset();

        //Save csAction type
        m_serializedData.AddInt((int)m_type);

        //Save the event data
        csAction.Save(m_serializedData);
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
            m_csAction.Load(stringList, 1);
        }
    }

    public override string ToString()
    {
        return (m_csAction != null) ? m_csAction.ToString() : "Action";
    }

}
