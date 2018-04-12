using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : Item
{
    [SerializeField]
    private StatusEffect m_statusEffect;

    public void Consume()
    {
        //Apply Effect to the character
        PlayerController.instance.character.ApplyStatusEffect(m_statusEffect);
    }

    public override void Interact()
    {
        Inventory.instance.AddToInventory(this);
    }
}
                  