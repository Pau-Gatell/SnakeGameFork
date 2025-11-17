using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    public virtual void Attack()
    {
        Debug.Log("Enemy attacks!");
    }

    List<Enemy> enemies = new List<Enemy>();
    private void Start()
    {
        Iterate();
    }

    public void Iterate()
    {
        RangedEnemy enemyRanged = new RangedEnemy();
        Enemy enemyRangedUpcast = (Enemy)enemyRanged;

        MeleeEnemy enemyMelee = new MeleeEnemy();
        Enemy enemyMeleeUpcast = (Enemy)enemyMelee;

        enemies.Add(enemyRangedUpcast);
        enemies.Add(enemyMeleeUpcast);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Attack();
        }
        // Esto no se puede hacer.
        //MeleeEnemy enemymeleeCast = enemyRanged;
        Enemy enemy2 = new RangedEnemy();
        MeleeEnemy downcast = (MeleeEnemy)enemy2;
    }
}

public class MeleeEnemy : Enemy
{
    public override void Attack()
    {
        Debug.Log("Melee enemy attacks with sword!");
    }
}
public class RangedEnemy : Enemy
{
    public override void Attack()
    {
        Debug.Log("Ranged enemy shoots arrows!");
    }
}
