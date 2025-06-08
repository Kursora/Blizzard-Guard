using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Animator elevator;
    [SerializeField] private GameObject startButton;
    [SerializeField] private RawImage snow;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bgm, elevatorSound;
    IEnumerator Init()
    {
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene("Game");
    }
    public void StartGame()
    {
        elevator.Play("elevator");
        audioSource.PlayOneShot(elevatorSound);
        startButton.SetActive(false);
        StartCoroutine(Init());    
    }

    void Update()
    {
        snow.uvRect = new Rect(snow.uvRect.x - 0.00001f, snow.uvRect.y +0.00001f, snow.uvRect.width, snow.uvRect.height);
    }
}
