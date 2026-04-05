using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VersionNumber : MonoBehaviour
{
    public bool isAlpha = false;
    private TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        if(isAlpha)
        {
            text.text = "Alpha " + Application.version;
        }
        else
        {
            text.text = Application.version;
        }
    }
}
