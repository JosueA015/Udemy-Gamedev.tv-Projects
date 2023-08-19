using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CrashDetector : MonoBehaviour
{
    [SerializeField] float Delay = 2f;
    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] ParticleSystem dustTrail;
    bool isConcussed = false;

    void OnTriggerEnter2D(Collider2D other)
   {
    
     if (other.gameObject.tag == "Ground" && !isConcussed)
     {
      isConcussed = true;
      crashEffect.Play();
      FindObjectOfType<Snowboarder>().disableControls();
      GetComponent<AudioSource>().Play();
      Invoke("ReloadScene", Delay);
     }
   }

   void ReloadScene()
   {
        SceneManager.LoadScene(0);
   }
}
