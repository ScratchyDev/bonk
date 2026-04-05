using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booklet : MonoBehaviour
{
    public GameObject[] pages;
    int currentPage;
        
    public void Next(){
        if(currentPage < pages.Length - 1){
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }
    }

    public void Previous(){
        if(currentPage > 0){
            pages[currentPage].SetActive(false);
            currentPage--;
            pages[currentPage].SetActive(true);
        }
    }
}
