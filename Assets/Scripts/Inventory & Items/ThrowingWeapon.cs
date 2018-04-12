using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class ThrowingWeapon : Weapon {

    private Rigidbody2D m_rigidBody;
    [SerializeField]
    private float m_speed = 5.0f;

    [SerializeField]
    private float m_returnDelay = 1.5f;
    private bool m_isReturning = false;
    private bool m_isOut = false;
    private StatusEffect m_statusEffect = null;
    private Collider2D childCol;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();

        m_rigidBody = GetComponent<Rigidbody2D>();
        childCol = transform.GetChild(0).GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            Destroy(gameObject);
            return;
        }

        if (m_owner != null)
        {
            m_rigidBody.velocity = Vector2.zero;
            transform.parent = m_owner.transform;
            transform.position = transform.parent.position;
        }
        else
        {
            Destroy(gameObject);
        }
     
    }


    void Update()
    {
       
        if(m_owner != null)
        {
            if (!m_isOut)
            {
                childCol.isTrigger = true;
                m_rigidBody.isKinematic = true;
            }
            else
            {
                childCol.isTrigger = false;
                m_rigidBody.isKinematic = false;
            }

            if (m_isReturning)
            {
                m_rigidBody.velocity = (m_owner.transform.position - transform.position).normalized * m_speed * 1.5f;
                childCol.isTrigger = true;
                if (m_owner.weapon != this)
                {
                    m_isReturning = false;
                    m_rigidBody.velocity = Vector2.zero;
                    transform.parent = m_owner.transform;
                    transform.position = transform.parent.position;
                }
            }

            if (Vector2.Distance(m_owner.transform.position, transform.position) < 0.5f && m_owner.weapon == this && m_isReturning)
            {
                m_isReturning = false;
                m_rigidBody.velocity = Vector2.zero;
                transform.parent = m_owner.transform;
                transform.position = transform.parent.position;
                m_isOut = false;
            }
        }
        
        if(m_isOut && m_owner == null)
        {
            m_isReturning = false;
            m_isOut = false;
            transform.position = player.transform.position;
            m_rigidBody.velocity = Vector2.zero;
        }
        

        if (transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Use this for initialization
    public override void Excecute()
    {
        if(!m_isOut)
        {
            m_rigidBody.velocity = transform.right * m_speed;
            m_isOut = true;
            Invoke("ReturnToPlayer", m_returnDelay);
            transform.parent = null;
        }
    }

    void ReturnToPlayer()
    {   
        m_isReturning = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {   
        if(m_owner == null || transform.parent != null)
        {
            return;
        }

        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            return;
        }

        Character character = other.gameObject.GetComponent<Character>();

      
        if (character)
        {
            character.TakeDamage((int)m_damage);
            if (m_statusEffect)
                character.ApplyStatusEffect(m_statusEffect);
        }
    }
}
