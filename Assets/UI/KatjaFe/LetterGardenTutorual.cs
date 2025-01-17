using CORE;
using CORE.Scripts;
using Scenes.Minigames.LetterGarden.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGardenTutorual : MonoBehaviour
{
    private KatjaFe katjafe;

    [SerializeField] private AudioClip Hey;
    [SerializeField] private AudioClip Intro;
    [SerializeField] private AudioClip followBee;
    [SerializeField] private AudioClip noBee;

    private bool waitingForInput = false;
    private DrawingHandler drawingHandler;

    private void Start()
    {
        katjafe = GetComponent<KatjaFe>();
        if (GameManager.Instance.PlayerData.TutorialLetterGarden)
        {

            katjafe.Initialize(false, followBee);
            return;
        }
        katjafe.Initialize(true, followBee);
        drawingHandler = FindFirstObjectByType<DrawingHandler>();
        Speak();
    }

    private void Speak()
    {
        katjafe.KatjaIntro(() =>
        {
            katjafe.KatjaSpeak(Hey, () =>
            {
                katjafe.KatjaSpeak(Intro, () =>
                {
                    if(true)
                    {
                        katjafe.KatjaSpeak(followBee, () =>
                        {
                            waitingForInput = true;
                        });
                    }
                    else
                    {
                        katjafe.KatjaSpeak(noBee, () =>
                        {
                            waitingForInput = true;
                        });
                    }
                    
                });
            });
        });
    }

    private void Update()
    {
        if (waitingForInput)
        {
            if (drawingHandler.IsTutorualOver)
            {
                waitingForInput = false;
                katjafe.KatjaSpeak(CongratsAudioManager.GetAudioClipFromDanishSet(Random.Range(0,CongratsAudioManager.GetLenghtOfAudioClipDanishList())), () =>
                {
                    katjafe.KatjaExit();
                    GameManager.Instance.PlayerData.TutorialLetterGarden = true;
                });
            }
        }
    }
}
