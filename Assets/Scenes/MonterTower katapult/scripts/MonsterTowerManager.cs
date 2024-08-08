using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterTowerManager : MonoBehaviour
{

    int ammo = 10;
    Dictionary<string, (Image, Image[])> questions;
    string currentQuestion;
    int currentQuestionIndex = 0;
    [SerializeField] TextMeshProUGUI displayBox;
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject noAmmoText;
    [SerializeField] GameObject[] ammoDisplay;
    [SerializeField] CatapultAming catapultAming;
    RaycastHit hit;
    Ray ray;
    [SerializeField] AmmoPupUp pupUp;

    /// <summary>
    /// call this to setup the minigame
    /// </summary>
    /// <param name="input">the dictionary that contains all the questions and images</param>
    public void SetDic(Dictionary<string, (Image, Image[])> input)
    {
        questions = input;

        currentQuestion = GetQuestion();
        SetDispay(currentQuestion);
    }

    void Start()
    {
        if (ammo <= 0)
        {
            noAmmoText.SetActive(true);
            for (int i = 0; i < ammoDisplay.Length; i++)
            {
                ammoDisplay[i].SetActive(false);
            }
            return;
        }

        if (ammo < ammoDisplay.Length)
        {
            for (int i = ammoDisplay.Length - 1; i >= ammo; i--)
            {
                ammoDisplay[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Cliced();
    }

    /// <summary>
    /// returns the next question
    /// </summary>
    /// <returns>the next question</returns>
    string GetQuestion()
    {
        return questions.ElementAt(currentQuestionIndex).Key;
    }

    /// <summary>
    /// updates the displaybox to the given string
    /// </summary>
    /// <param name="textToDispay">the string the displaybox is set to</param>
    void SetDispay(string textToDispay)
    {
        displayBox.text = textToDispay;
    }

    /// <summary>
    /// sends out a ray to detect what was cliced on.
    /// also handels the check if you cliced on the right image/stone
    /// </summary>
    void Cliced()
    {
        if(ammo <= 0) return;
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.transform != null)
        {
            //do stuff with cheking if the click is right or wrong
            catapultAming.shoot(hit.point);
            return;
            currentQuestionIndex++;
            currentQuestion = GetQuestion();
            SetDispay(currentQuestion);
            RemoveAmmo();
        }
    }

    /// <summary>
    /// removes ammo and updates the pile of ammo
    /// </summary>
    void RemoveAmmo()
    {
        ammo--;
        if(ammo < ammoDisplay.Length)
            ammoDisplay[ammo].SetActive(false);
        if (ammo <= 0)
            noAmmoText.SetActive(true);
    }

    /// <summary>
    /// shows the tooltip for the amount of ammo the player has
    /// </summary>
    private void OnMouseEnter()
    {
        pupUp.SetAndShowToolTip(ammo.ToString());
    }

    /// <summary>
    /// hide the tooltip
    /// </summary>
    private void OnMouseExit()
    {
        pupUp.HideToolTip();
    }
}
