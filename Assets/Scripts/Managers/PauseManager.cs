using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseUI;
    public GameObject optionsUI;

    float resumeTimeScale = 1f;

    bool IsPaused {
        get { return pauseUI != null && pauseUI.activeSelf; }
    }

    void Update(){
        // Escape: close the options, otherwise toggle the pause menu.
        // (Ignored while the level is frozen by StartPaused, so it cannot skip the start screen.)
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(optionsUI != null && optionsUI.activeSelf){
                optionsUI.SetActive(false);
            }
            else if(IsPaused){
                Play();
            }
            else if(Time.timeScale > 0f){
                Pause();
            }
        }
    }

#if !UNITY_EDITOR
    void OnApplicationFocus(bool focus){
        if(!focus && !IsPaused && Time.timeScale > 0f){
            Pause();
        }
    }
#endif
    
    public void Pause(){
        if(Time.timeScale > 0f){
            // remember slow-motion (scope) so resuming does not silently cancel it;
            // ignore the tiny hit-stop scale
            resumeTimeScale = Time.timeScale < 0.1f ? 1f : Time.timeScale;
        }
        pauseUI.SetActive(true);
        optionsUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Play(){
        pauseUI.SetActive(false);
        optionsUI.SetActive(false);
        Time.timeScale = resumeTimeScale;
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
