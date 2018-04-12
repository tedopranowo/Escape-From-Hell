using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour {

    public static Inventory instance;

    [SerializeField]
    Weapon[] m_weaponList;
    [SerializeField]
    Consumables[] m_consumableList;
    [SerializeField]
    private bool[] m_keys = new bool[(int)Key.Type.kCount];
  
    private InventoryUI m_inventoryUI;
    private int m_currentWeaponIndex = 0;
    private int m_currentConsumableIndex = 0;
    private Shoot m_shoot = null;

    private static Character player
    {
        get
        {
            return PlayerController.instance.character;
        }
    }

    public int currentConsumableIndex
    {
        get
        {
            return m_currentConsumableIndex;
        }
    }



    void Awake()
    {
        foreach (Selectable selectableUI in Selectable.allSelectables)
        {
            Debug.Log(selectableUI.name);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        m_inventoryUI = GetComponent<InventoryUI>();
        if (m_inventoryUI)
            m_inventoryUI.Init(m_weaponList.Length, m_consumableList.Length);
    
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(m_shoot)
        {
            m_inventoryUI.UpdateReloadUI(m_shoot.lastShot + m_shoot.fireRate, m_shoot.fireRate);         
        }
    }
    /// <summary>
    /// Function for adding item
    /// </summary>
    /// <param name="item"></param>
    public void AddToInventory(Weapon item)
    {
        if (CheckHaveItem(item))
        {
            return;
        }

        for (int i = 0; i < m_weaponList.Length; i++)
        {
            if(m_weaponList[i] == null)
            {
                m_weaponList[i] = item;
                m_weaponList[i].gameObject.SetActive(false);
                m_inventoryUI.UpdateWeaponUI(i, item);

                ChangeCurrentWeapon(i);


                m_weaponList[i].transform.parent = player.transform;
                m_weaponList[i].transform.position = player.transform.position;

                m_weaponList[i].owner = player;

                return;
            }
        }
        //If inventory is full swap currently equipped item with it
        SwapItem(item);
    }
    public void AddToInventory(Consumables item)
    {
        for (int i = 0; i < m_consumableList.Length; i++)
        {
            if (m_consumableList[i] == null)
            {
                m_consumableList[i] = item;
                m_consumableList[i].gameObject.SetActive(false);
                if (m_inventoryUI)
                    m_inventoryUI.UpdateConsumablesUI(i, item);

                ChangeCurrentConsumables(i);
                return;
            }
        }
        //If inventory is full swap currently equipped item with it
        SwapItem(item);
    }

    public void AddToInventory(Key.Type keyType)
    {
        m_keys[(int)keyType] = true;
        m_inventoryUI.ShowKeyUI((int)keyType, true);
    }

    /// <summary>
    /// Function for removing weapon
    /// </summary>
    /// <param name="index"></param>
    public void RemoveWeapon(int index)
    {
        m_inventoryUI.UpdateWeaponUI(m_currentWeaponIndex, null);
        m_weaponList[index].owner = null;
        m_weaponList[index] = null;    
    }
    /// <summary>
    /// Function for removing consumable
    /// </summary>
    /// <param name="index"></param>
    public void RemoveConsumable(int index)
    {
        m_consumableList[index] = null;
    }

   public void DestroyConsumable(int index)
    {
        m_inventoryUI.UpdateConsumablesUI(currentConsumableIndex, null);
        Destroy(m_consumableList[index].gameObject);
        m_consumableList[index] = null;
    }

    /// <summary>
    /// Function for swapping item in inventory
    /// </summary>
    /// <param name="item"></param>
    public void SwapItem(Weapon item)
    {
        if (CheckHaveItem(item))
        {         
            return;
        }

        m_weaponList[m_currentWeaponIndex].gameObject.SetActive(true);
        m_weaponList[m_currentWeaponIndex].transform.position = player.transform.position;
        m_weaponList[m_currentWeaponIndex].transform.eulerAngles = Vector3.zero;
        m_weaponList[m_currentWeaponIndex].transform.parent = null;
        RemoveWeapon(m_currentWeaponIndex);
        AddToInventory(item);
    }

    public void SwapItem(Consumables item)
    {
        m_consumableList[m_currentConsumableIndex].gameObject.SetActive(true);
        m_consumableList[m_currentConsumableIndex].transform.position = player.transform.position;
        m_consumableList[m_currentConsumableIndex].transform.eulerAngles = Vector3.zero;
        m_consumableList[m_currentConsumableIndex].transform.parent = null;
        RemoveConsumable(m_currentConsumableIndex);
        AddToInventory(item);
    }


    public void ChangeCurrentWeapon(int index)
    {
        if (m_weaponList[index] == null)
        {
            return;
        }

        if (m_inventoryUI)
        m_inventoryUI.HighlightWeaponUI(m_currentWeaponIndex, index);
         m_weaponList[m_currentWeaponIndex].gameObject.SetActive(false);
        m_currentWeaponIndex = index;
        player.GetComponent<Character>().weapon = m_weaponList[index];
        m_weaponList[index].gameObject.SetActive(true);

        m_shoot = m_weaponList[m_currentWeaponIndex].GetComponent<Shoot>();

        if(m_shoot)
        {
            m_inventoryUI.ShowReloadUI(true, m_shoot.fireRate); 
        }
        else
        {
            m_inventoryUI.ShowReloadUI(false, 0);
        }
    }

    public void ChangeCurrentConsumables(int index)
    {
        if (m_consumableList[index] == null)
        {
            return;
        }

        if (m_inventoryUI)
        m_inventoryUI.HighlightConsumableUI(currentConsumableIndex, index);
        m_currentConsumableIndex = index;
        player.GetComponent<Character>().consumable = m_consumableList[index];
    }

    public bool UseKey(Key.Type keyType)
    {
        //If there exist the type of key in inventory
        if (m_keys[(int)keyType] == true)
        {
            //Use the key and return true
            m_keys[(int)keyType] = false;
            m_inventoryUI.ShowKeyUI((int)keyType, false);
            return true;
        }

        //There is no specified key in inventory
        return false;
    }

    public void DropWeapon()
    {
        m_weaponList[m_currentWeaponIndex].transform.position = player.transform.position;
        m_weaponList[m_currentWeaponIndex].transform.eulerAngles = Vector3.zero;
        m_weaponList[m_currentWeaponIndex].transform.parent = null;
        PlayerController.instance.character.weapon = null;
        RemoveWeapon(m_currentWeaponIndex);
    }

    public bool CheckHaveItem(Weapon weapon)
    {
        for(int i = 0; i < m_weaponList.Length; i++)
        {
            if(m_weaponList[i] != null)
            {
                if(m_weaponList[i].itemName == weapon.itemName)
                {
                    return true;
                }
            }
        }
    
        return false;
    }
}
