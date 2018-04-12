using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class ProjectileScript : MonoBehaviour
{

    protected Rigidbody2D m_rigidbody2D = null;

    protected Vector2 m_direction = Vector2.zero;

    [SerializeField]
    protected float m_speed = 10.0f;

    [SerializeField]
    protected bool m_isPiercing = false;



    [SerializeField]
    protected float m_duration = 3.0f;


    [SerializeField]
    protected float m_damage = 0.0f;

    [SerializeField]
    [Range(0.1f, 100.0f)]
    protected float m_pierceDamageDropPercent = 0.1f;

    [SerializeField]
    protected StatusEffect m_statusEffect = null;


    public float damage
    {
        set { m_damage = value; }
    }

    public StatusEffect statusEffect
    {
        set
        {
            m_statusEffect = value;
        }
    }

    public float speed
    {
        set
        {
            m_speed = value;
        }
        get
        {
            return m_speed;
        }
    }

    public bool isPiercing
    {
        set
        {
            m_isPiercing = value;
        }
        get
        {
            return m_isPiercing;
        }
    }

    protected virtual void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        Invoke("Destroy", m_duration);
    }
    public Vector2 direction { set { m_direction = value; } }



    protected virtual void Destroy()
    {
        Destroy(gameObject);
    }


    protected virtual void Update()
    {
        m_rigidbody2D.velocity = m_direction * m_speed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Character character = other.gameObject.GetComponent<Character>();
        if (character)
        {
            character.TakeDamage((int)m_damage);
            if (m_statusEffect)
                character.ApplyStatusEffect(m_statusEffect);
            if(!m_isPiercing)
            {
                Destroy();
            }
            else
            {
                m_damage -= (m_pierceDamageDropPercent / 100 * m_damage);
            }
        }

        if (other.tag == "Wall")
        {
            Destroy();
        }
    }
}
