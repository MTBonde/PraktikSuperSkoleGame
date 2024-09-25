using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour
{
    [SerializeField] Sprite grov;
    [SerializeField] Sprite fabrik;
    [SerializeField] Sprite have;
    [SerializeField] Sprite racerbane;
    [SerializeField] Sprite shop;
    [SerializeField] Sprite tower;
    [SerializeField] Sprite player;
    [SerializeField] Sprite home;

    private Image thisImage;

    public void ChangeImage(string imageName)
    {
        thisImage = this.GetComponent<Image>();
        if (thisImage == null)
        {
            Debug.Log("No Image component on map Icon.");
        }

        switch (imageName.ToLower())
        {
            case "grov":
                thisImage.sprite = grov;
                break;
            case "fabrik":
                thisImage.sprite = fabrik;
                break;
            case "have":
                thisImage.sprite = have;
                break;
            case "racerbane":
                thisImage.sprite = racerbane;
                break;
            case "shop":
                thisImage.sprite = shop;
                break;
            case "tower":
                thisImage.sprite = tower;
                break;
            case "player":
                thisImage.sprite = player;
                break;
            case "home":
                thisImage.sprite = home;
                break;
            default:
                Debug.LogError("Invalid image name: " + imageName);
                break;
        }
        Debug.Log("Checked");
    }

    public void HighLight()
    {

    }
}
