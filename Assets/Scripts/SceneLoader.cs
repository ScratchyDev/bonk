using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public SaveManager saveManager;
    
    public void LoadScene(int i){
        saveManager.SavePlayer();
        SceneManager.LoadScene(i);
    }
}
