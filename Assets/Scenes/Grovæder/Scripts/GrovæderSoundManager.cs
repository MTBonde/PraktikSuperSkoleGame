using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grov�derSoundController : MonoBehaviour
{

    [SerializeField]
    private AudioSource audioLetterSource;

    
    private AudioClip letterSoundClip;


    //so we can update letterSoundClip via other scipts
    public void SetGrov�derSound(AudioClip clip)
    {
        letterSoundClip = clip;
        
    }

    // Update is called once per frame
    void Update()
    {

        //plays the audioLetterSource once by pressing space
        if (Input.GetKey(KeyCode.Space))
        {
            
            audioLetterSource.PlayOneShot(letterSoundClip);
        }
    }
}
