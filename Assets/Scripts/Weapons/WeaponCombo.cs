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
    private int baseDamage;
    private bool boosted;

    void Awake(){
        baseDamage = damage;
    }

    public override void WeaponFunction(){
        if(boosted){
            return;
        }

        currentCombo++;

        if(currentCombo >= comboRequirement){
            if(comboSlider != null){
                comboSlider.maxValue = comboTime;
            }
            currentCombo = 0;
            currentComboTime = comboTime;
            boosted = true;
            damage = baseDamage * 2;
            GameFeel.PopText(transform.position + Vector3.up * 2f, "COMBO x2!", new Color(1f, 0.85f, 0.2f), 6f);
        }
    }

    void FixedUpdate(){
        if(comboText != null){
            comboText.text = currentCombo.ToString();
        }

        if(boosted){
            currentComboTime -= Time.deltaTime;

            // Before this fix the damage was halved on every physics step while no combo was
            // active, so it dropped to 0 almost immediately. Now it is restored exactly once.
            if(currentComboTime <= 0f){
                currentComboTime = 0f;
                boosted = false;
                damage = baseDamage;
            }
        }

        if(comboSlider != null){
            comboSlider.value = currentComboTime;
        }
    }
}
