using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class PlayerColorManager : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;


    public void ColorChange(string colorName)
    {
        Color selectedColor;

        //register farven
        switch (colorName.ToLower())
        {
            case "orange":
                selectedColor = HexToColor("ead25f");
                    break;
            case "blue":
                selectedColor = HexToColor("19daf9");
                break;
            case "red":
                selectedColor = HexToColor("cf5b5d");
                break;
            case "green":
                selectedColor = HexToColor("6aa85c");
                break;
            default:
                selectedColor = Color.white;
                break;
        }

        //�ndre de specefikke knogler
        switch (skeletonAnimation.skeletonDataAsset.name)
        {
            case "PraktikMonster_SkeletonData":
                string[] slotsToColor = 
                {
                    "Monster L lowerleg color",
                    "Monster R lowerleg color",
                    "Monster L upperleg color",
                    "Monster R upperleg color",
                    "Monster head",
                    "Monster body",
                    "Monster R upperarm color",
                    "Monster L upperarm color",
                    "Monster R lowerarm color",
                    "Monster L lowerarm color"
                };

                foreach (string slotName in slotsToColor)
                {
                    ChangeSlotColor(slotName, selectedColor);
                }
                
                break;

            default:
                break;
        }
        
        //send den til manageren
        //gameSetup.ChosenMonsterColor = colorName;
    }

    private void ChangeSlotColor(string slotName, Color color)
    {
        var slot = skeletonAnimation.Skeleton.FindSlot(slotName);
        if (slot != null)
        {
            slot.SetColor(color);
        }
    }


    private Color HexToColor(string hex)
    {
        //Unity's colorUtility klasse, der fors�ger at konverter en string i HTML style hex format
        if(ColorUtility.TryParseHtmlString("#" + hex, out Color color))
        {
            //hvis farvekoden matcher
            return color;
        }
        else
        {
            //hvis farvekoden ikke matcher
            return Color.white;
        }
    }
}
