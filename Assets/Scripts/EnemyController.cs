using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private List<EnemyBehavior> enemies = new List<EnemyBehavior>();

    public void TakeDamage(EnemyBehavior damagedEnemy)
    {
        if (damagedEnemy.GetHealth() <= 0)
        {
            Dead(damagedEnemy);
        }
        else
        {
            damagedEnemy.DecreaseHealth();
        }
    }

    public void Dead(EnemyBehavior deadObject)
    {
        Destroy(deadObject);
    }

    public void UpdateEnemiesPositions()
    {
        foreach (EnemyBehavior enemy in enemies)
        {
            //enemy.UpdatePosition();
        }
    }
}
