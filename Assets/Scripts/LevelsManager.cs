using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    public Button[] levelButtons;
    public SaveManager saves;

    public void UpdateButtons(){
        for (int i = 0; i < levelButtons.Length; i++){
            if(i < saves.completionLevel + 1){
                levelButtons[i].interactable = true;
            } else {
                levelButtons[i].interactable = false;
            }
        }
    }
}
