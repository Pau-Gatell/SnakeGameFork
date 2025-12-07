using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private HealthController health;

    void Start()
    {
        health = GetComponent<HealthController>();

    }

    public void TakeDamage(float dmg)
    {
        if (health != null)
        {
            health.UpdateHealth(dmg);
        }
    }
}