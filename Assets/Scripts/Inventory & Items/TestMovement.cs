using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour {

    [SerializeField]
    GameObject m_collidingItem = null;
    Rigidbody2D m_rigidBody;
    [SerializeField]
    private float m_speed = 3f;

    [SerializeField]
    private Weapon m_weapon;

    public Weapon weapon
    {
        set
        {
            m_weapon = value;
            m_weapon.transform.parent = transform;
            m_weapon.transform.position = new Vector3(transform.position.x + 0.5f, 0, 0);
        }
    }

    // Use this for initialization
    void Start () {
        m_rigidBody = GetComponent<Rigidbody2D>();

    }

    void Shoot()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_rigidBody.velocity = new Vector2(horizontal * m_speed, vertical * m_speed);

        SelectHotKey();

        if (Input.GetKeyDown(KeyCode.E) && m_collidingItem != null)
        {
            Inventory.instance.AddToInventory(m_collidingItem.GetComponent<Weapon>());
            m_collidingItem = null;
        }

       

        if(m_weapon != null)
        {
            if (Input.GetMouseButton(0))
            {

                m_weapon.Excecute();
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Transform weaponTrans = m_weapon.transform;
            Vector3 targetDir = mousePos - transform.position;
            float weaponAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

            weaponTrans.localEulerAngles = new Vector3(0, 0, weaponAngle);
            
            if (Mathf.Abs(weaponAngle) > 92)
            {
                weaponTrans.localScale = new Vector3(1, -1, 1);
                m_weapon.transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
            }
            else if(Mathf.Abs(weaponAngle) < 88)
            {
                weaponTrans.localScale = new Vector3(1, 1, 1);
                m_weapon.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            }

          
        }
   
    }

 
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Weapon")
        {
            if(m_collidingItem == null)
            {
                m_collidingItem = other.gameObject;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Weapon")
        {
            if (m_collidingItem != null)
            {
                m_collidingItem = null;
            }
        }
    }

    void SelectHotKey()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.ChangeCurrentWeapon(0);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.instance.ChangeCurrentWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Inventory.instance.ChangeCurrentWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            Inventory.instance.ChangeCurrentConsumables(0);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Inventory.instance.ChangeCurrentConsumables(1);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            Inventory.instance.ChangeCurrentConsumables(2);
        }
    }

  
}
