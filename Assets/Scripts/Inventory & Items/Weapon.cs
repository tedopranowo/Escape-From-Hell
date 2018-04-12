using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[Tedo]:
 * If you plan to make every weapon shoot a projectile (no melee),
 * why don't you combine Weapon class with Shoot class instead? */
[RequireComponent(typeof(BoxCollider2D))]
public class Weapon : Item {

    [SerializeField]
    protected float m_damage = 20;
    [SerializeField]
    protected Character m_owner = null;

    public Character owner
    {
        get
        {
            return m_owner;
        }
        set
        {
            m_owner = value; 
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
    public virtual void Excecute()
    {   
    
    }

    public override void Interact()
    {
        Inventory.instance.AddToInventory(this);
    }


    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (m_owner || Inventory.instance.CheckHaveItem(this))
        {
            return;
        }
        //Check if the colliding object is controlled by player
        PlayerController playerController = collision.GetComponent<PlayerController>();

        //If the colliding object is controlled by player
        if (playerController != null)
        {
            playerController.interactableObject = this;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (m_owner || Inventory.instance.CheckHaveItem(this))
        {
            return;
        }
        //Check if the colliding object is controlled by player
        PlayerController playerController = collision.GetComponent<PlayerController>();

        //If the colliding object is controlled by player
        if (playerController != null)
        {
            playerController.interactableObject = null;
        }
    }

}
