using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen : MonoBehaviour
{
    [SerializeField] private bool ouvert = false;
    [SerializeField] private Animation doorAnimation;
    
    private AudioSource sound;
    public AudioClip openSound;
    public AudioClip closeSound;
    
    void Start()
    {
        doorAnimation = GetComponentInParent<Animation>();
        sound = GetComponent<AudioSource>();
    }

    public void ChangeEtat()
    {
        if (ouvert == false)
        {
            sound.clip = openSound;
            sound.Play();
            doorAnimation.Play("ouvrir");
            ouvert = true;
        }
        else
        {
            sound.clip = closeSound;
            sound.Play();
            doorAnimation.Play("fermer");
            ouvert = false;
        }
    }
}
