using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public SaveManager saveManager;
    
    public void LoadScene(int i){
        if(saveManager.completionLevel >= i - 1){
            saveManager.SavePlayer();
            SceneManager.LoadScene(i);
        }
    }
}
