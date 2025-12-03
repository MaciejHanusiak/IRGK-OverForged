using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManuManagement : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
        Analytics.Instance.EndGame();
    }
}
