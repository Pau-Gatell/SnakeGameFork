using UnityEngine.SceneManagement;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public void PlayGame()
    {
        // Carrega la teva escena de joc
        SceneManager.LoadScene("spmap_level1");
    }

    public void QuitGame()
    {
        // Tanca el joc
        Application.Quit();

    }
 
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}