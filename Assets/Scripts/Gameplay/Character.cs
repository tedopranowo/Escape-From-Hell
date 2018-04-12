using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : MonoBehaviour
{
    //---------------------------------------------------------------------------------------------
    // Variables
    //---------------------------------------------------------------------------------------------
    private Dictionary<string, StatusEffect> m_statusEffects = new Dictionary<string, StatusEffect>();
    private List<StatusEffect> m_toBeRemovedStatusEffects = new List<StatusEffect>();

    [SerializeField]
    private int m_maxHealth = 100;
    private int m_health;

    [SerializeField]
    private float m_startingDamageMultiplier = 1.0f;
    private float m_damageMultiplier;
    
    [SerializeField]
    private float m_movementSpeed;

    [SerializeField]
    private Weapon m_weapon;

    [SerializeField]
    private Consumables m_consumable;

    private SpriteRenderer m_spriteRenderer;
    private Color m_originalColor;
    private bool m_colorChange;
    private float m_colorChangeTimer = 0.0f;

    private const float k_hitColorDuration = 0.25f;

    [SerializeField]
    private float m_weaponXoffset;


    private Animator m_anim;
    private Rigidbody2D m_rigidBody;
    //---------------------------------------------------------------------------------------------
    // Properties
    //---------------------------------------------------------------------------------------------

    public int health
    {
        get { return m_health; }
    }

    public int maxHealth
    {
        get { return m_maxHealth; }
    }
    public float damageMultiplier
    {
        get { return m_damageMultiplier; }
        set { m_damageMultiplier = value; }
    }

    public float moveSpeed
    {
        get { return m_movementSpeed; }
        set { m_movementSpeed = value; }
    }

    public Weapon weapon
    {
        set
        {   
            m_weapon = value;
        }

        get
        {
            return m_weapon;
        }
    }

    public Consumables consumable
    {
        set
        {
            m_consumable = value;
            m_consumable.transform.parent = transform;
            m_consumable.transform.localPosition = Vector3.zero;
        }
        get { return m_consumable; }
    }

    public Dictionary<string, StatusEffect> statusEffects
    {
        get
        {
            return m_statusEffects;
        }
    }

    //---------------------------------------------------------------------------------------------
    // Unity Overrides
    //---------------------------------------------------------------------------------------------
    virtual protected void Awake()
    {
        m_health = m_maxHealth;
        m_damageMultiplier = m_startingDamageMultiplier;
        m_anim = GetComponent<Animator>();
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (!m_spriteRenderer)
        {
            FindRendererInChildren(transform);
        }
        m_originalColor = m_spriteRenderer.color;
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (m_weapon)
        {
            Inventory.instance.AddToInventory(m_weapon);
        }
    }

    protected void Update()
    {
        //TODO: REFACTOR

        if (m_weapon != null)
        {   
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Transform weaponTrans = weapon.transform;
            Vector3 targetDir = mousePos - transform.position;
            float weaponAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

            weaponTrans.eulerAngles = new Vector3(0, 0, weaponAngle);
      

            if (Mathf.Abs(weaponAngle) > 92)
            {
                if (m_weapon.GetComponent<ThrowingWeapon>() == null)
                {
                    weaponTrans.localScale = new Vector3(1, -1, 1);
                    m_weapon.transform.position = new Vector3(transform.position.x - m_weaponXoffset, transform.position.y, transform.position.z);
                }
           
               
            }
            else if (Mathf.Abs(weaponAngle) < 88)
            {
             
                if (m_weapon.GetComponent<ThrowingWeapon>() == null)
                {
                    weaponTrans.localScale = new Vector3(1, 1, 1);
                    m_weapon.transform.position = new Vector3(transform.position.x + m_weaponXoffset, transform.position.y, transform.position.z);
                }
           
            }

        }
 

        //END TODO

        OnHitColorTick();

        UpdateStatusEffect();
    }

    //---------------------------------------------------------------------------------------------
    // Functions
    //---------------------------------------------------------------------------------------------

    public void Move(Vector2 direction)
    {
        m_rigidBody.velocity = new Vector3(direction.x, direction.y, 0);


        if (direction.x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction.x < 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);

        }
        if (m_anim != null)
        {
            m_anim.SetBool("isRunning", direction != Vector2.zero);
        }

    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        //If the player has that status effect already, refresh the duration
        if (m_statusEffects.ContainsKey(statusEffect.hoveredTitle))
        {
            Debug.Log("Duration of " + statusEffect.hoveredTitle + " is refreshed");

            //Refresh the duration of the status effect currently applied on the player
            m_statusEffects[statusEffect.hoveredTitle].RefreshDuration();
        }
        else
        {
            Debug.Log("Applied: " + statusEffect.hoveredTitle);

            //Create a copy of the status effect
            StatusEffect newStatusEffect = ScriptableObject.CreateInstance<StatusEffect>();
            newStatusEffect.CopyFrom(statusEffect);

            //Apply the status effect copy to the player
            m_statusEffects[newStatusEffect.hoveredTitle] = newStatusEffect;
            newStatusEffect.OnApplied(this);
        }
    }

    private void UpdateStatusEffect()
    {
        //Apply Tick for every status effects
        foreach(KeyValuePair<string, StatusEffect> kvp in m_statusEffects)
        {
            kvp.Value.OnTick(this);
        }

        //Remove all status effects that are marked as to be removed
        foreach (StatusEffect statusEffect in m_toBeRemovedStatusEffects)
        {
            Debug.Log("Removed: " + statusEffect.name);
            m_statusEffects.Remove(statusEffect.hoveredTitle);
            Destroy(statusEffect);
        }
        m_toBeRemovedStatusEffects.Clear();
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        statusEffect.OnRemoved(this);

        //Mark the status effect as to be removed
        m_toBeRemovedStatusEffects.Add(statusEffect);
    }

    public void Heal(int heal)
    {
        m_health += heal;
        if (m_health > m_maxHealth)
        {
            m_health = m_maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        //Only take damage in debug build
        Debug.Log("Taking " + damage + " damage");
        m_health -= damage;
        OnHitColor(Color.red);
        if (m_health <= 0)
            Die();
    }

    private void OnHitColor(Color color)
    {
        m_spriteRenderer.color = color;
        m_colorChange = true;
    }

    private void OnHitColorTick()
    {
        if (m_colorChange)
        {
            if (m_colorChangeTimer < k_hitColorDuration)
            {
                m_colorChangeTimer += Time.deltaTime;
            }
            else
            {
                m_spriteRenderer.color = m_originalColor;
                m_colorChangeTimer = 0;
                m_colorChange = false;
            }
        }
    }

    private void Die()
    {
 
        Destroy(gameObject);

        if (gameObject.tag == "Player")
        {
            SceneManagerSingleton.instance.LoadGameOver();
        }
    }

    //find renderer within children
    private void FindRendererInChildren(Transform child)
    {
        m_spriteRenderer = child.GetComponent<SpriteRenderer>();
        if (!m_spriteRenderer)
        {
            for (int i = 0; i < child.childCount; ++i)
            {
                if (!m_spriteRenderer)
                { 
                    FindRendererInChildren(child.GetChild(i));
                }
            }
        }
    }
}
