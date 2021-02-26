using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractibleUI : MonoBehaviour
{
    public Text interactibleText;
    public Text itemText;
    public RawImage itemImage;

    public void SetItemImage(Sprite image)
    {
        itemImage.texture = image.texture;
    }
}
