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
        CheckAndSetBar();
    }

    public void SetFiller(float currentHP, float maxHP)
    {
        CurrentHP = currentHP;
        MaxHP = maxHP;
        if(fillerImage!=null)
            fillerImage.fillAmount = currentHP / maxHP;
        CheckAndSetBar();
    }


    void CheckAndSetBar()
    {
        if (gameObject != null)
        {
            if (CurrentHP == MaxHP)
            {
                gameObject.SetActive(false);
            }
            else if (CurrentHP != MaxHP)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
