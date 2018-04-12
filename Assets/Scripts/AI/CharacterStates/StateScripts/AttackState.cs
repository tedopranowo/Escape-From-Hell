using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : CharacterState
{
    public override void OnEnter(Controller controller, CharacterStateData data)
    {
        base.OnEnter(controller, data);
        AttackStateData stateData = (data as AttackStateData);
        stateData.timer = new Timer(stateData.cooldownTime);
        if(!stateData.target)
            stateData.target = (stateData.findTransform.state as FindTargetState).FindTarget();
        (data as AttackStateData).timer.ResetTimer();
    }

    public override void OnTick(Controller controller, CharacterStateData data)
    {
        (data as AttackStateData).timer.Tick();
        if ((data as AttackStateData).timer.status == Timer.Status.finished)
        {
            PerformAttack(controller, data);
            //reset the attack timer.
            (data as AttackStateData).timer.ResetTimer();
        }
    }

    /*[Tedo]:
     * How about changing the 'data' type to 'AttackStateData'? */
    public virtual void PerformAttack(Controller controller, CharacterStateData data)
    {
        for (int i = 0; i < (data as AttackStateData).projectiles; ++i)
        {
            if ((data as AttackStateData).projectiles % 2 != 0)
            {
                Attack(controller, data, (data as AttackStateData).angle * (i - Mathf.Floor((data as AttackStateData).projectiles / 2)));
            }
            else
            {
                Attack(controller, data, (data as AttackStateData).angle * ((i - ((float)(data as AttackStateData).projectiles / 2.0f)) + 0.5f));
            }
        }
    }

    public virtual void Attack(Controller controller, CharacterStateData data, float angle)
    {
        AttackStateData attackData = data as AttackStateData;

        //direction of attack from us towards the target.
        if (!attackData.target)
            return;
        Vector3 delta = attackData.target.position - controller.transform.position;

        //normalize the direction of attack.
        delta.Normalize();
        //rotate the attack by the specified angle
        delta = (new Vector2(delta.x, delta.y)).Rotate(angle);
        //offset the spawn location by the distance specified
        delta *= attackData.spawnDistance;

        //rotation of the spawned projectile is the direction of the attack.
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, delta);

        //spawn the projectile.
        GameObject projectile = Instantiate(attackData.projectile, controller.transform.position + delta, rotation, controller.transform.parent);
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();

        //set the projectile layer to enemy bullet
        projectile.layer = LayerMask.NameToLayer("EnemyBullet");

        //assign velocity to the projectile.
        projectileScript.direction = delta.normalized;

        //Set the projectile damage and status effect
        projectileScript.damage = attackData.damage;
        projectileScript.statusEffect = attackData.statusEffectOnHit;
        projectileScript.speed = attackData.projectileSpeed;
    }

    public override void OnExit(Controller controller)
    {
        base.OnExit(controller);
    }
}
