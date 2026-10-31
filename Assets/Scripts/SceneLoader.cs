using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public SaveManager saveManager;
    
    public void LoadScene(int i){
        // a scoped (slow-motion) or paused level must not leak its time scale into the next scene
        Time.timeScale = 1f;
        saveManager.SavePlayer();
        SceneManager.LoadScene(i);
    }

    public void ReloadScene(){
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
