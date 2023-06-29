using System.Collections;
using UnityEngine;

public class WeaponCombo : BaseWeapon
{
    public int comboRequirement;
    public float comboTime;
    int currentCombo;

    public override void WeaponFunction(){
        currentCombo++;

        if(currentCombo >= comboRequirement){
            currentCombo = 0;
            StartCoroutine(Combo());
        }
    }

    IEnumerator Combo(){
        damage++;
        yield return new WaitForSeconds(comboTime);
        damage--;
    }
}