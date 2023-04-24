using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basketball : MonoBehaviour
{
    private AudioManager audioManager;
    private GameManager gameManager;
    private AudioSource audioSource;

    private float minimumCollisionSoundVelocity = 1;

    private void Start()
    {
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent <GameManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= minimumCollisionSoundVelocity)
        {
            if (collision.gameObject.CompareTag("Backboard"))
            {
                audioManager.PlaySound(audioSource, "Backboard Bounce");
            } else
            {
                audioManager.PlaySoundRandomPitch(audioSource, "Basketball Bounce");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.AddToScore(1);
    }

    public void PrimeDestroyBall()
    {
        StartCoroutine(DestroyBall());
    }

    IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
