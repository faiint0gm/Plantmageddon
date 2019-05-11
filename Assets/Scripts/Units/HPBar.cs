using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private Image fillerImage;

    public void SetFiller(float currentHP, float maxHP)
    {
        fillerImage.fillAmount = currentHP / maxHP;
    }
}
