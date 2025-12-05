using System.Collections;
using UnityEngine;

public class BombBirdController : BirdController
{
    [Header("Explosion Settings")]
    public float explosionDelay = 1f;
    public float explosionRadius = 3f;
    public float explosionForce = 500f;
    public GameObject explosionEffect;

    private bool _hasExploded = false;

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (!isActive || _hasExploded) return;

        DetectAlive();
        DrawTrace();

        // Player tap mid-air to explode early
        if (Input.GetMouseButtonDown(0))
        {
            Explode();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_hasExploded)
        {
            StartCoroutine(DelayedExplosion());
        }
    }

    IEnumerator DelayedExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;

        // Particle FX
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Physics AOE
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            Rigidbody2D rb = hit.attachedRigidbody;
            if (rb != null)
            {
                Vector2 dir = rb.position - (Vector2)transform.position;
                rb.AddForce(dir.normalized * explosionForce);
            }

            // Destroy pigs or fragile objects
            var enemy = hit.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(999);
            }
        }

        // Clean camera target
        if (CameraController.instance != null)
            CameraController.instance.ClearTarget();

        // Load next bird
        if (SlingshotController.instance != null)
            SlingshotController.instance.Reload();

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}