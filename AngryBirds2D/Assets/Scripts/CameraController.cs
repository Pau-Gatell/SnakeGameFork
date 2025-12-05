using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("Configuració càmera")]
    public float smoothSpeed = 0.125f;
    public float minX = -20f;
    public float maxX = 20f;

    private Transform target;
    private Vector3 initialPosition;

    private void Awake()
    {
        // Assignem la instància tan aviat com sigui possible
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Hi ha més d'una CameraController a l'escena. S'elimina la nova.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        initialPosition = transform.position;
    }

    private void Update()
    {
        // No intentis accedir a target si és null
        if (target == null) return;

        // Protecció addicional: si el Transform ha estat destruït (Unity intern), comprovar null
        if (target.Equals(null))
        {
            target = null;
            return;
        }

        Vector3 desiredPosition = new Vector3(
            Mathf.Clamp(target.position.x, minX, maxX),
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }

    public void SetTarget(Transform newTarget)
    {
        if (newTarget == null)
        {
            Debug.LogWarning("CameraController.SetTarget rebut amb null.");
            target = null;
            return;
        }

        target = newTarget;
    }

    public void ClearTarget()
    {
        // comprovació defensiva abans d'assignar null
        target = null;
    }

    public void ResetCamera()
    {
        target = null;
        transform.position = initialPosition;
    }
}