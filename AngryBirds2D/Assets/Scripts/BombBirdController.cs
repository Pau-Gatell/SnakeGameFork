using System.Collections;
using UnityEngine;

public class BombBirdController : BirdController
{
    [Header("Bomb Settings")]
    public float explosionDelay = 1.2f;
    public float explosionRadius = 3f;
    public float explosionForce = 600f;

    public GameObject explosionEffect;
    public AudioClip explosionSound;

    private bool hasExploded = false;
    private bool hasStartedCountdown = false;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        DrawTrace();
        DetectAlive();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasStartedCountdown)
        {
            hasStartedCountdown = true;
            StartCoroutine(DelayedExplosion());
        }
    }

    IEnumerator DelayedExplosion()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    private void OnMouseDown()
    {
        if (isActive && !hasExploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        // Partícules
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // So
        if (explosionSound)
            AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);

        // Registrar objectes dins el radi
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            Rigidbody2D rb = hit.attachedRigidbody;

            if (rb != null)
            {
                Vector2 dir = rb.position - (Vector2)transform.position;
                rb.AddForce(dir.normalized * explosionForce);
            }

            HealthController health = hit.GetComponent<HealthController>();
            if (health != null)
            {
                health.UpdateHealth(50f); // Dany configurable
            }
        }

        Destroy(gameObject);
        ReloadNext();   // Notifica al slingshot perquè carregui el següent ocell
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}