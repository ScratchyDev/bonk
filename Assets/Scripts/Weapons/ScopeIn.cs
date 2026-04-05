using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ScopeIn : MonoBehaviour
{
    public Camera cam;
    public GameObject ScopeUI;
    public Transform gun;

    [HideInInspector] public bool scoped;

    void Update(){
        if (!IsPointerOverUIObject()) {
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = cam.transform.position.z + cam.nearClipPlane;
            transform.position = mousePosition;
            if(scoped){
                // Vector3 perpendicular = gun.position + mousePosition;
                // gun.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
                var mouseScreenPos = Input.mousePosition;
                var startingScreenPos = cam.WorldToScreenPoint(gun.position);
                mouseScreenPos.x -= startingScreenPos.x;
                mouseScreenPos.y -= startingScreenPos.y;
                var angle = Mathf.Atan2(mouseScreenPos.y, mouseScreenPos.x) * Mathf.Rad2Deg;
                gun.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90));
            }
        }
    }

    public void ScopeEnter(){
        scoped = true;
        ScopeUI.SetActive(true);
        gun.gameObject.GetComponent<Animator>().enabled = false;
        Time.timeScale = 0.25f;
    }

    public void ScopeExit(){
        scoped = false;
        ScopeUI.SetActive(false);
        gun.gameObject.GetComponent<Animator>().enabled = true;
        Time.timeScale = 1f;
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
