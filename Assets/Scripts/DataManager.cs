using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public SaveManager saveManager;

    public void Reset(){
        saveManager.completionLevel = 0;
        saveManager.SavePlayer();
    }

    public void UnlockAll(){
        saveManager.completionLevel = 1000;
        saveManager.SavePlayer();
    }
}
