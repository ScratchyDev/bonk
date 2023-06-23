using UnityEngine;
using System;
using TMPro;

public class CountDownTimer : MonoBehaviour {
    float countdown;
    public TextMeshProUGUI timeText;

    void Update(){
        if(countdown > 0)
        countdown -= Time.deltaTime;
        timeText.text = (Math.Round(countdown, 1).ToString() + "s");
    }

    public void StartTimer(float time){
        countdown = time;
    }
}
