using CORE.Scripts;
using Scenes.Minigames.MonsterTower.Scrips;
using Scenes.Minigames.MonsterTower.Scrips.MTGameModes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootPicture : MonoBehaviour, IMTGameMode
{
    /// <summary>
    /// Will be called by the TowerManager to create a brick with the correct answer
    /// </summary>
    /// <param name="str">the correct answer will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetCorrectAnswer(string str, TowerManager manager)
    {
        manager.soloImage.texture = ImageManager.GetImageFromWord(str);
    }

    /// <summary>
    /// Will be called by the TowerManager to create a brick with an (usually random) incorrect answer
    /// </summary>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void SetWrongAnswer(TowerManager manager)
    {
        var rndImageWithKey = ImageManager.GetRandomImageWithKey();

        manager.soloImage.texture = rndImageWithKey.Item1;
        manager.ImageKey = rndImageWithKey.Item2;
    }

    /// <summary>
    /// Sets the answer key, which will tell the player which brick is correct. Uses the opposite medium of SetCorrectAnswer
    /// </summary>
    /// <param name="str">The answer key will have to take a string to find the correct image using the ImageManager, or have a string for text</param>
    /// <param name="manager">a reference back to the tower manager so it can modify the tower manager</param>
    public void GetDisplayAnswer(string str, TowerManager manager)
    {
        manager.displayBox.text = str;
    }

    /// <summary>
    /// create a series of answers
    /// </summary>
    /// <param name="count">number of answers to create</param>
    /// <returns>Returns a set of answers strings to be used by the towerManager</returns>
    public string[] GenerateAnswers(int count)
    {
        string[] returnedString = new string[count];
        for (int i = 0; i < count; i++)
        {
            returnedString[i] = WordsForImagesManager.GetRandomWordForImage();
        }
        return returnedString;
    }
    /// <summary>
    /// changes the prefab of the TowerManager so we only apply 1 image to the bricks
    /// </summary>
    /// <param name="manager">a reference back to the towermanager</param>
    public void SetAnswerPrefab(TowerManager manager)
    {
        manager.AnswerHolderPrefab = manager.SingleImageHolderPrefab;
        manager.soloImage = manager.SingleImageHolderPrefab.transform.GetChild(0).GetComponent<RawImage>();
    }
}
