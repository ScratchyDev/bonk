using UnityEngine;
using System.Globalization;
using TMPro;

public class CountDownTimer : MonoBehaviour {
    float countdown;
    public TextMeshProUGUI timeText;

    void Update(){
        if(countdown > 0)
        countdown -= Time.deltaTime;
        timeText.text = Mathf.Max(0f, countdown).ToString("0.0", CultureInfo.InvariantCulture) + "s";
    }

    public void StartTimer(float time){
        countdown = time;
    }
}
