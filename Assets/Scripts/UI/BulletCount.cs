using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BulletCount : MonoBehaviour
{
    BetterFighter betterFighter;
    [SerializeField] CanvasGroup bulletCOuntUI;
    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        bulletCOuntUI.blocksRaycasts = false;
        bulletCOuntUI.interactable = false;
        betterFighter = FindObjectOfType<BetterFighter>();
    }

    // Update is called once per frame
    void Update()
    {
        if(betterFighter.leftHandWeapon != null)
        {
            if (betterFighter.leftHandWeapon.isRanged)
            {
                bulletCOuntUI.alpha = 1;
                text.text = betterFighter.GetBulletAmt().ToString();
            }
            else
            {
                bulletCOuntUI.alpha = 0;
            }
        }
        else
        {
            bulletCOuntUI.alpha = 0;
        }
    }
    
}
