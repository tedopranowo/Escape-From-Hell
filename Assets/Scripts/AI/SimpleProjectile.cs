using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple temporary projectile for enemy use.
public class SimpleProjectile : MonoBehaviour
{
    Rigidbody2D m_rigidbody2D = null;

    /*[Tedo]:
     * I think m_direction is a more appropriate name */
    [SerializeField]
    Vector2 m_velocity = Vector2.zero;

    [SerializeField]
    float m_speed = 1.0f;

    [SerializeField]
    float m_life = 10.0f;

    [SerializeField]
    int m_damage = 20;

    public Vector2 velocity{get { return m_velocity; }set { m_velocity = value; }}

    private void Start()
    {
        //destroy self after m_life time
        Destroy(gameObject, m_life);
    }

    void Update()
    {
        if (!m_rigidbody2D)
        {
            m_rigidbody2D = GetComponent<Rigidbody2D>();
        }
        if (!m_rigidbody2D)
            return;
        m_rigidbody2D.velocity = Vector2.Lerp(m_rigidbody2D.velocity, m_velocity * m_speed, 0.1f);
        if (m_velocity != Vector2.zero)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, m_rigidbody2D.velocity);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.transform.tag == "Player")
        //{
        Character character = other.gameObject.GetComponent<Character>();
        if(character)
            character.TakeDamage(m_damage);
        //}
    }

}
