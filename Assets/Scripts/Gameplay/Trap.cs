using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    private int m_damageDone;

    [SerializeField]
    private StatusEffect m_statusEffectApplied;

    [SerializeField]
    private float m_durationActive = 3.0f;

    private float m_timer = 0.0f;

    bool m_active = true;

    SpriteRenderer m_spriteRenderer;
    BoxCollider2D m_collider;
    Animator m_anim;
    private void Awake()
    {
        m_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_collider = gameObject.GetComponent<BoxCollider2D>();
        m_anim = GetComponent<Animator>();
        //StartCoroutine(ActivateAndDeactivate());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            Character playerCharacter = PlayerController.instance.character;

            playerCharacter.TakeDamage(m_damageDone);
            playerCharacter.ApplyStatusEffect(m_statusEffectApplied);
        }
    }

    private void Update()
    {
        m_timer += Time.deltaTime;

        if (m_timer >= m_durationActive)
        {
            if (m_active)
            {
                m_anim.Play("TrapDeactivate");
                m_collider.enabled = false;
                m_active = false;

            }
            else
            {
                m_anim.Play("TrapActivate");
                m_collider.enabled = true;
                m_active = true;
            }

            m_timer = 0;
        }
    }

    //IEnumerator ActivateAndDeactivate()
    //{
    //    while (true)
    //    {
    //        if (m_active)
    //        {
    //            m_spriteRenderer.enabled = false;
    //            m_collider.enabled = false;
    //            m_active = false;

    //        }
    //        else
    //        {
    //            m_spriteRenderer.enabled = true;
    //            m_collider.enabled = true;
    //            m_active = true;
    //        }
    //        yield return new WaitForSeconds(m_durationActive);
    //    }
    //}


}
