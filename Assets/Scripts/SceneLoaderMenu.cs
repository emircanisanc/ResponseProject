using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderMenu : MonoBehaviour
{
    bool isLoading = false;

    public void QuitGame()
    {
        if (isLoading) return;
        
        Application.Quit();
    }
    
    public void OpenScene1()
    {
        OpenScene(1);
    }

    public void OpenScene2()
    {
        OpenScene(2);
    }

    public void OpenScene3()
    {
        OpenScene(3);
    }

    public void OpenScene(int scene)
    {
        if (isLoading) return;

        isLoading = true;

        SceneManager.LoadScene(scene);
    }
}
