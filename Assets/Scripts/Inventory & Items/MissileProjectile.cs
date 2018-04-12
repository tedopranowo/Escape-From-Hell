using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : ProjectileScript {

    [SerializeField]
    private LayerMask m_layerMask;
    [SerializeField]
    private float m_homingRadius = 3.0f;

    [SerializeField]
    private float m_rotationSpeed = 5.0f;


    private Collider2D m_target = null;

    protected override void Update()
    {
        base.Update();

        if(!m_target)
              m_target = Physics2D.OverlapCircle(transform.position, m_homingRadius, m_layerMask);

        if (m_target != null)
        {
            Vector3 dir = m_target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion target = Quaternion.AngleAxis(angle -90, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * m_rotationSpeed);
            m_rigidbody2D.velocity = transform.up * m_speed;
        }
    }
}
