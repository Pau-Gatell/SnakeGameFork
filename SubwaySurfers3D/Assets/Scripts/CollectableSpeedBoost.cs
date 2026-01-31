using UnityEngine;

public class SpeedBoostPU : MonoBehaviour
{
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().ActivateSpeedBoost(duration);
            Destroy(gameObject);
        }
    }
}