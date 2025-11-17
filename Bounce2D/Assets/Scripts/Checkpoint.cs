using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint current;

    public static int totalCheckpoints;
    public static int checkpointsActivats;

    private bool activat = false;

    void Start()
    {
        if (totalCheckpoints == 0)
        {
            totalCheckpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).Length;
            checkpointsActivats = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !activat)
        {
            activat = true;
            current = this; //Checkpoint on es reapareix
            checkpointsActivats++;

            gameObject.SetActive(false);

            Debug.Log($"Checkpoint activat! ({checkpointsActivats}/{totalCheckpoints})");
        }
    }

    public static bool TotsActivats()
    {
        return checkpointsActivats >= totalCheckpoints;
    }
}