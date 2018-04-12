using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    //---------------------------------------------------------------------------------------------
    // Singleton
    //---------------------------------------------------------------------------------------------
    private static PlayerController s_instance = null;
    public static PlayerController instance
    {
        set
        {
            Debug.Assert(value != null);

            if (s_instance != null)
            {
                Debug.LogWarning("Singleton object cannot be instatiated twice!");
                Destroy(value);
                return;
            }

            s_instance = value;
        }
        get
        {
            return s_instance;
        }
    }

    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    [SerializeField]
    private Interactable m_interactableObject;

    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------
    public Interactable interactableObject
    {
        set
        {
            m_interactableObject = value;
        }
        get
        {
            return m_interactableObject;
        }
    }

    //---------------------------------------------------------------------------------------------
    // Unity Monobehavior
    //---------------------------------------------------------------------------------------------
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        UpdateMovement();
        GetWeaponHotBarKeyDown();
        GetItemHotBarKeyDown();
        GetActionKeyDown();
        GetDropItemKeyDown();
        GetFireKeyDown();
        GetUseItemKeyDown();
        GetDropWeaponKeyDown();
    }

    //---------------------------------------------------------------------------------------------
    // Functions
    //---------------------------------------------------------------------------------------------
    public void UpdateMovement()
    {
        float xMovPos;
        float yMovPos;
        xMovPos = Input.GetAxisRaw("Horizontal") * character.moveSpeed;
        yMovPos = Input.GetAxisRaw("Vertical") * character.moveSpeed;


        Vector2 moveVector = new Vector2(xMovPos, yMovPos);
        character.Move(moveVector);
    }

    public void GetActionKeyDown()
    {
        //If action button is pressed and there is object to interact with
        if (Input.GetButtonDown("ActionKey") && interactableObject)
        {
            interactableObject.Interact();
        }
    }

    public void GetDropItemKeyDown()
    {
        if (Input.GetButtonDown("DropItemKey"))
        {
            Debug.Log("DropItemKey");
        }
    }

    public void GetWeaponHotBarKeyDown()
    {
        if (Input.GetButtonDown("weapon1"))
        {
            Inventory.instance.ChangeCurrentWeapon(0);
        }

        if (Input.GetButtonDown("weapon2"))
        {
            Inventory.instance.ChangeCurrentWeapon(1);
        }

        if (Input.GetButtonDown("weapon3"))
        {
            Inventory.instance.ChangeCurrentWeapon(2);
        }

    }

    public void GetItemHotBarKeyDown()
    {
        if (Input.GetButtonDown("ItemHotBar"))
        {
            Inventory.instance.ChangeCurrentConsumables(0);
        }

        if (Input.GetButtonDown("ItemHotBar2"))
        {
            Inventory.instance.ChangeCurrentConsumables(1);
        }

        if (Input.GetButtonDown("ItemHotBar3"))
        {
            Inventory.instance.ChangeCurrentConsumables(2);
        }
    }

    public void GetFireKeyDown()
    {
        if (Input.GetButton("Fire1"))
        {
            if (character.weapon)
                character.weapon.Excecute();
        }
    }

    public void GetDropWeaponKeyDown()
    {
        if (Input.GetButtonDown("DropItemKey"))
        {
            //This cause bug
            /*
            if (character.weapon)
                Inventory.instance.DropWeapon();
                */
        }
    }

    public void GetUseItemKeyDown()
    {
        if (Input.GetButtonDown("UseItemKey"))
        {
            if (character.consumable)
            {
                character.consumable.Consume();
                Debug.Log("Consuming Item");
                Inventory.instance.DestroyConsumable(Inventory.instance.currentConsumableIndex);
            }
        }
    }
}
