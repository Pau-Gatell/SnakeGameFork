using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenus : MonoBehaviour

{
    public void Continue()
    {
        SceneManager.LoadScene("spmap_tiling");
    }
    public void Play()
    {
        SceneManager.LoadScene("spmap_tiling");
    }
    public void Quit()
    {
        Application.Quit();
    }
        
}