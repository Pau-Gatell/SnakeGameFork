using System.Collections;
using UnityEngine;

public class BombBirdController : BirdController
{
    [Header("Bomb Settings")]
    public float explosionDelay = 1.2f; //Delay abans d’explotar
    public float explosionRadius = 3f; //Radi de l'explosió
    public float explosionForce = 600f; 

    public GameObject explosionEffect; //Efecte de l'explosió
    public AudioClip explosionSound; //Audio de l'explosió

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

    private void OnMouseDown() //Si fas click al ratolí explota el bird
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

        //Partícules
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        //So
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
                health.UpdateHealth(50f); //Mal
            }
        }

        Destroy(gameObject); //Destrueix el bird
        ReloadNext();   //Es carrega el seguent ocell per posar-se a la slingshot
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; //Dibuixa el radi de l'explosió del bird
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}