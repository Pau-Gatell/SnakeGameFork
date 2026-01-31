using UnityEngine;

public class CollectableDoubleCoins : MonoBehaviour
{
    public float duration = 6f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.ActivateDoubleCoins(duration);
            Destroy(gameObject);
        }
    }
}