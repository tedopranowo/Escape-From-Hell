using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class ShootingWeapon : Weapon {

    private Shoot m_shoot;

    protected override void Awake()
    {
        base.Awake();

        m_shoot = GetComponent<Shoot>();
    }

    public override void Excecute()
    {
        m_shoot.Excecute();
        m_shoot.damage = m_damage;
    }
}
