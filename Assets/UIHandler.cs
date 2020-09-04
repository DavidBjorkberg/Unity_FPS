using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    public Text pickUpText;
    public Text ammoText;
    public static UIHandler instance;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
    public void ShowPickUpText()
    {
        pickUpText.gameObject.SetActive(true);
    }
    public void HidePickUpText()
    {
        pickUpText.gameObject.SetActive(false);
    }
    public void UpdateAmmoText(int maxAmmo, int currentAmmo)
    {
        ammoText.text = maxAmmo + " / " + currentAmmo;
    }
}
