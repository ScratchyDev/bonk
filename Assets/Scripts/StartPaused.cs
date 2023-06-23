using UnityEngine;

public class StartPaused : MonoBehaviour
{
    void Start(){
        Time.timeScale = 0f;
    }

    public void UnPause(){
        Time.timeScale = 1f;
    }
}
