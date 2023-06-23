using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseUI;
    public GameObject optionsUI;
    
    public void Pause(){
        pauseUI.SetActive(true);
        optionsUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Play(){
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Options(){
        optionsUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Exit(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
