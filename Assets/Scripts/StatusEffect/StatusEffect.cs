using System.Collections.Generic;
using UnityEngine;

//-------------------------------------------------------------------------------------------------
// StatusEffect
//-------------------------------------------------------------------------------------------------
[CreateAssetMenu(fileName = "New Status Effect", menuName = "Custom/Status Effect", order = 1)]
public class StatusEffect : ScriptableObject, IHoverableDescription {

    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    //This variables will be used for status effect UI in the future. Temporarily disabling it
    [SerializeField] private string m_name = "";
    [SerializeField] private string m_description = "";
    [SerializeField] private Sprite m_icon = null;
    [SerializeField] private float m_duration = 0.0f;
    [SerializeField] private CSEffect[] m_effects = null;

    private float m_endTime;

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    public string hoveredTitle { get { return m_name; } }
    public string hoveredDescription { get { return m_description; } }
    public Sprite icon
    {
        get
        {
            return m_icon;
        }
    }

    //---------------------------------------------------------------------------------------------
    // Constructor
    //---------------------------------------------------------------------------------------------
    public StatusEffect(StatusEffect copyFrom)
    {
        CopyFrom(copyFrom);
    }

    //---------------------------------------------------------------------------------------------
    // Functions
    //---------------------------------------------------------------------------------------------
    public void OnApplied(Character owner)
    {
        m_endTime = Time.time + m_duration;

        foreach (CSEffect effect in m_effects)
            effect.OnApplied(owner);
    }

    public void OnTick(Character owner)
    {
        foreach (CSEffect effect in m_effects)
            effect.OnTick(owner);

        //Remove the effect if the duration is over
        if (m_endTime < Time.time)
            owner.RemoveStatusEffect(this);
    }

    public void OnRemoved(Character owner)
    {
        foreach (CSEffect effect in m_effects)
            effect.OnRemoved(owner);
    }

    public void RefreshDuration()
    {
        m_endTime = Time.time + m_duration;
    }

    public void CopyFrom(StatusEffect copySource)
    {
        m_name = copySource.m_name;
        m_description = copySource.m_description;
        m_icon = copySource.m_icon;
        m_duration = copySource.m_duration;

        m_effects = new CSEffect[copySource.m_effects.Length];
        for (int i = 0; i < copySource.m_effects.Length; ++i)
        {
            m_effects[i] = new CSEffect(copySource.m_effects[i]);
        }
    }
}

//-------------------------------------------------------------------------------------------------
// Comparer class for StatusEffect
//-------------------------------------------------------------------------------------------------
public class StatusEffectEqualityComparer : IEqualityComparer<StatusEffect>
{
    //A hacky way to get the hash set value
    private StatusEffect m_val1;

    //Get the m_val1
    public StatusEffect hashSetValue
    {
        get
        {
            return m_val1;
        }
    }

    public bool Equals(StatusEffect val1, StatusEffect val2)
    {
        m_val1 = val1;

        //If both are null, they are equal
        if (val1 == null && val2 == null)
            return true;
        //If one of them is null, they are not equal
        else if (val1 == null || val2 == null)
            return false;
        //If they have the same name, they are equal
        else if (val1.hoveredTitle == val2.hoveredTitle)
            return true;
        //If they don't have same name, they are not equal
        else
            return false;
    }

    public int GetHashCode(StatusEffect obj)
    {
        int hashCode = obj.hoveredTitle.GetHashCode();

        return obj.name.GetHashCode();
    }
}

//-------------------------------------------------------------------------------------------------
// Extension method for Status Effect's hash set
//-------------------------------------------------------------------------------------------------
public static class StatusEffectHashSetExtension
{
    public static StatusEffect GetValue(this HashSet<StatusEffect> hashSet, StatusEffect value)
    {
        bool hasTheStatusEffect = hashSet.Contains(value);

        if (hashSet.Contains(value))
            return (hashSet.Comparer as StatusEffectEqualityComparer).hashSetValue;
        else
            return null;
    }
}