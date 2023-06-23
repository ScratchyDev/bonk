using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour {

    [Header("Variables")]
    public float maxEnergy;
    public float energy;
    public float rechargeRate;

    [Header("References")]
    public Slider energyBar;

    void Start(){
        energyBar.maxValue = maxEnergy;
        energy = maxEnergy / 8;
    }

    void FixedUpdate(){
        if(energy < maxEnergy){
            energy += rechargeRate / 10;
        }

        energyBar.value = energy;
    }

    public void TakeEnergy(float amount){
        energy -= amount;
    }
}
