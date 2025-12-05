using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private HealthController health;

    void Start()
    {
        health = GetComponent<HealthController>();

        if (health == null)
        {
            Debug.LogError("⚠ EnemyController needs a HealthController attached!");
        }
    }

    public void TakeDamage(float dmg)
    {
        if (health != null)
        {
            health.UpdateHealth(dmg);
        }
    }
}