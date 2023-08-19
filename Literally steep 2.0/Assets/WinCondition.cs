using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    [SerializeField] float Delay = 2f;
    [SerializeField] ParticleSystem finishEffect;
   private void OnTriggerEnter2D(Collider2D other) 
   {
    
     if (other.tag == "Player")
     { 
        finishEffect.Play();
        GetComponent<AudioSource>().Play();
        Invoke("ReloadScene", Delay);
     }
   }

   void ReloadScene()
   {
        SceneManager.LoadScene(0);
   }
}
