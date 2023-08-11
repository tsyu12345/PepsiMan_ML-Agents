using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CanEffect : MonoBehaviour {

    public AudioClip sound;
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        //Debug.Log("AudioSource {0}", audioSource);
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("Get Can!");
        audioSource.PlayOneShot(sound);
        Destroy(gameObject, 0.25f);
        //gameObject.SetActive(false);
    }




}
