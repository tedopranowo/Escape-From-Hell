using UnityEngine;

[System.Serializable]
public class CSAction : CustomizableScript {
    public virtual void ApplyAction(Character target) { }

    public virtual void Save(CustomSerializedData serializedData)
    {
    }

    public virtual void Load(string[] serializedData, int startIndex)
    {

    }
}

//-------------------------------------------------------------------------------------------------
// Type of CSAction
//-------------------------------------------------------------------------------------------------

public class CSActionDealDamage : CSAction
{
    [SerializeField] private int m_damage = 0;

    public override void ApplyAction(Character target)
    {
        target.TakeDamage(m_damage);
    }

    public override string ToString()
    {
        return "Deal " + m_damage + " damage to owner";
    }

    public override void Save(CustomSerializedData serializedData)
    {
        serializedData.AddInt(m_damage);
    }

    public override void Load(string[] serializedData, int startIndex)
    {
        m_damage = int.Parse(serializedData[startIndex]);
    }
}

public class CSActionHeal : CSAction
{
    [SerializeField] private int m_healAmount = 0;

    public override void ApplyAction(Character target)
    {
        target.Heal(m_healAmount);
    }

    public override string ToString()
    {
        return "Heal " + m_healAmount + " hp to owner";
    }

    public override void Save(CustomSerializedData serializedData)
    {
        serializedData.AddInt(m_healAmount);
    }

    public override void Load(string[] serializedData, int startIndex)
    {
        m_healAmount = int.Parse(serializedData[startIndex]);
    }
}

public class CSActionModifyMovementSpeed : CSAction
{
    enum Type
    {
        kUnit,
        kPercentage
    }

    [SerializeField] private float m_amount = 0.0f;
    [SerializeField] private Type m_unitType = Type.kUnit;

    public override void ApplyAction(Character target)
    {
        float totalChanges = 0.0f;
        //If the the unit type is percentage
        if (m_unitType == Type.kPercentage)
        {
            totalChanges = target.moveSpeed * m_amount * 0.01f;
        }
        //If the unit type is normal
        else if (m_unitType == Type.kUnit)
        {
            totalChanges = m_amount;
        }

        target.moveSpeed += totalChanges;
    }

    public override string ToString()
    {
        string text = (m_amount >= 0) ? "Increase" : "Reduce";
        text += " movement speed by ";
        text += Mathf.Abs(m_amount);
        switch (m_unitType)
        {
            case Type.kUnit:
                text += " units";
                break;
            case Type.kPercentage:
                text += " percents";
                break;
        }

        return text;
    }

    public override void Save(CustomSerializedData serializedData)
    {
        serializedData.AddFloat(m_amount);
        serializedData.AddInt((int)m_unitType);
    }

    public override void Load(string[] serializedData, int startIndex)
    {
        m_amount = float.Parse(serializedData[startIndex]);
        m_unitType = (Type)int.Parse(serializedData[startIndex + 1]);
    }
}