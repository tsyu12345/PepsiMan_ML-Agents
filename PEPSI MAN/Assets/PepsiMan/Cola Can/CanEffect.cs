using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanEffect : MonoBehaviour {

    public AudioClip sound;
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Get Can!");
        PlayDisposeEffect();
        
    }

    private void PlayDisposeEffect() {
        audioSource.PlayOneShot(sound);
        Destroy(gameObject);
    }


}
