using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public Text pickUpText;
    public Text ammoText;
    public Player player;
    public Slider healthSlider;
    public Slider ammoSlider;
    public static GameManager instance;

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
    public void UpdateHealthBar(float maxHealth, float curHealth)
    {
        healthSlider.value = curHealth / maxHealth;
    }
    public void UpdateAmmoBar(float maxAmmo, float curAmmo)
    {
        ammoSlider.value = curAmmo / maxAmmo;
    }
    public void ShowPickUpText()
    {
        pickUpText.gameObject.SetActive(true);
    }
    public void HidePickUpText()
    {
        pickUpText.gameObject.SetActive(false);
    }
}
