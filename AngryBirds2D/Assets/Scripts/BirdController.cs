using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Rigidbody2D Rbody { get; private set; }
    public bool isActive;

    private bool isDead;
    private float minYToDie = -20f;

    public virtual void Initialize()
    {
        Rbody = GetComponent<Rigidbody2D>();
        Rbody.bodyType = RigidbodyType2D.Kinematic;
        isActive = false;
        isDead = false;
    }

    private void Update()
    {
        if (!isActive || isDead)
            return;

        DetectAlive();
        DrawTrace();
    }

    public void SetBirdActive(bool value)
    {
        isActive = value;
    }

    /// <summary>
    /// Checks if the bird is still alive on screen.
    /// </summary>
    public void DetectAlive()
    {
        if (isDead)
            return;

        if (transform.position.y < minYToDie || Rbody.linearVelocity.magnitude < 0.1f)
        {
            StartCoroutine(DestroyAfterDelay());
            isDead = true;
        }
    }

    private System.Collections.IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        // Notify camera to stop following THIS bird
        if (CameraController.instance != null)
            CameraController.instance.ClearTarget();

        // Tell slingshot to load next bird
        SlingshotController.instance.Reload();

        Destroy(gameObject);
    }

    public void DrawTrace()
    {
        // Dibuixa l�nia o part�cules, si tens implementaci�
    }

    private void OnDestroy()
    {
        if (CameraController.instance != null)
            CameraController.instance.ClearTarget();
    }
}
