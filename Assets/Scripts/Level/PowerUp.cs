using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// A pick-up that bobs in place. Give it a sprite and a Collider2D and it becomes a trigger by itself.
/// The player (anything with a Movement component) collects it by touching it.
[RequireComponent(typeof(Collider2D))]
public class PowerUp : MonoBehaviour
{
    public enum Kind { Energy, Speed, Damage }

    public Kind kind = Kind.Energy;
    [Tooltip("Energy: how much energy is refilled.")]
    public float amount = 100f;
    [Tooltip("Speed / Damage: the multiplier while the power-up lasts.")]
    public float multiplier = 1.5f;
    [Tooltip("Speed / Damage: how long it lasts, in seconds.")]
    public float duration = 8f;
    [Tooltip("0 = one use. Otherwise it reappears after this many seconds.")]
    public float respawnTime = 0f;
    [Tooltip("Tint the sprite with the colour of the power-up kind.")]
    public bool autoTint = true;

    Vector3 basePosition;
    bool available = true;
    Renderer[] renderers;
    Collider2D trigger;

    void Awake(){
        trigger = GetComponent<Collider2D>();
        trigger.isTrigger = true;
        renderers = GetComponentsInChildren<Renderer>();
        basePosition = transform.position;

        if(autoTint){
            foreach(SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>()){
                sprite.color = KindColour();
            }
        }
    }

    void Update(){
        if(!available) return;
        transform.position = basePosition + Vector3.up * (Mathf.Sin(Time.time * 3f) * 0.15f);
    }

    Color KindColour(){
        switch(kind){
            case Kind.Speed: return new Color(1f, 0.9f, 0.3f);
            case Kind.Damage: return new Color(1f, 0.4f, 0.35f);
            default: return new Color(0.4f, 0.8f, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(!available) return;

        Movement movement = other.GetComponentInParent<Movement>();
        if(movement == null) return;

        Collect(movement);
    }

    void Collect(Movement movement){
        Vector3 popPosition = transform.position + Vector3.up;

        switch(kind){
            case Kind.Energy:
                if(movement.energyManager != null) movement.energyManager.Refill(amount);
                GameFeel.PopText(popPosition, "+ENERGY", KindColour(), 5f);
                break;

            case Kind.Speed:
                movement.speedMultiplier *= multiplier;
                GameFeel.PopText(popPosition, "SPEED UP!", KindColour(), 5f);
                GameFeel.Run(ExpireSpeed(movement, multiplier, duration));
                break;

            case Kind.Damage:
                List<BaseWeapon> weapons = WeaponsOf(movement);
                foreach(BaseWeapon weapon in weapons) weapon.damageMultiplier *= multiplier;
                GameFeel.PopText(popPosition, "POWER UP!", KindColour(), 5f);
                GameFeel.Run(ExpireDamage(weapons, multiplier, duration));
                break;
        }

        if(respawnTime > 0f){
            StartCoroutine(Respawn());
        }
        else{
            Destroy(gameObject);
        }
    }

    // weapons that use the same EnergyManager as the player belong to the player
    List<BaseWeapon> WeaponsOf(Movement movement){
        List<BaseWeapon> result = new List<BaseWeapon>();
        foreach(BaseWeapon weapon in BonkUtil.FindAll<BaseWeapon>()){
            if(weapon.energyManager == movement.energyManager) result.Add(weapon);
        }
        if(result.Count == 0){
            result.AddRange(BonkUtil.FindAll<BaseWeapon>());
        }
        return result;
    }

    static IEnumerator ExpireSpeed(Movement movement, float amount, float seconds){
        yield return new WaitForSeconds(seconds);
        if(movement != null) movement.speedMultiplier /= amount;
    }

    static IEnumerator ExpireDamage(List<BaseWeapon> weapons, float amount, float seconds){
        yield return new WaitForSeconds(seconds);
        foreach(BaseWeapon weapon in weapons){
            if(weapon != null) weapon.damageMultiplier /= amount;
        }
    }

    IEnumerator Respawn(){
        available = false;
        trigger.enabled = false;
        foreach(Renderer r in renderers) r.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        foreach(Renderer r in renderers) r.enabled = true;
        trigger.enabled = true;
        available = true;
    }
}
