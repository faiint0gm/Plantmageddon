using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private Image fillerImage;
    float CurrentHP, MaxHP;

    public void SetupHPBar(float currentHP, float maxHP)
    {
        CurrentHP = currentHP;
        MaxHP = maxHP;
    }

    public void SetFiller(float currentHP, float maxHP)
    {
        CurrentHP = currentHP;
        MaxHP = maxHP;
        fillerImage.fillAmount = currentHP / maxHP;
    }

    private void Update()
    {
        if (CurrentHP / MaxHP == 1 && gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
