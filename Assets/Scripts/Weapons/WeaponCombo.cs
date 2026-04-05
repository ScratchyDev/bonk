using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponCombo : BaseWeapon
{
    public int comboRequirement;
    public float comboTime;
    public TMP_Text comboText;
    public Slider comboSlider;
    private int currentCombo;
    private float currentComboTime;


    public override void WeaponFunction(){
        currentCombo++;

        if(currentCombo >= comboRequirement){
            comboSlider.maxValue = comboTime;
            currentCombo = 0;
            currentComboTime = comboTime;
            damage = damage * 2;
        }
    }

    void FixedUpdate(){
        comboText.text = currentCombo.ToString();
        comboSlider.value = currentComboTime;

        currentComboTime -= Time.deltaTime;
        if(currentComboTime <= 0){
            damage = damage / 2;
        }
    }
}