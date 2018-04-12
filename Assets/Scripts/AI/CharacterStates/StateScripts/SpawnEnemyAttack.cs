using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyAttack : AttackState
{
   
    public override void Attack(Controller controller, CharacterStateData data, float angle)
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
       
    }

}
