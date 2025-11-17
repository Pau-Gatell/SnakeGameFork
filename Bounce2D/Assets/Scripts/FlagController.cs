using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Només permet guanyar si has agafat tots els checkpoints
            if (Checkpoint.TotsActivats())
            {
                Debug.Log("Has ganado!");
                SceneManager.LoadScene("WinMenu");
            }
            else
            {
                Debug.Log("No has activado todos los checkpoints!");
            }
        }
    }
}