using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //UI Elements
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countdownText;
    public GameObject titleScreen;
    public GameObject endScreen;
    public TextMeshProUGUI finalScoreText;

    //Game Audio
    public AudioSource gameMusic;
    public AudioSource menuMusic;
    public AudioSource pointScoreAudio;
    public AudioSource shotClockAudio;

    public CinemachineFreeLook gameCamera;

    private GameObject player;
    private GameObject basketballHoop;

    public float startingTime = 60;
    private int score;
    private float timer;
    private bool gameIsActive = false;
    private bool shotClockIsActive = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        basketballHoop = GameObject.Find("Basketball Hoop");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowCursor();
        }

        if (gameIsActive)
        {
            CountTime();
        }
    }

    public void StartGame()
    {
        gameIsActive = true;
        player.GetComponent<PlayerController>().ToggleGameActive(gameIsActive);
        timer = startingTime;
        score = 0;
        AddToScore(0);
        player.GetComponent<PlayerController>().SpawnBall();
    }

    private void EndGame()
    {
        gameIsActive = false;
        player.GetComponent<PlayerController>().ToggleGameActive(gameIsActive);
        gameMusic.Stop();
        endScreen.gameObject.SetActive(true);
        finalScoreText.text = score.ToString();
        ShowCursor();
        scoreText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        gameCamera.gameObject.SetActive(false);
    }

    public void AddToScore(int amountToAdd)
    {
        if (gameIsActive)
        {
            score += amountToAdd;
            scoreText.text = "Score: " + score;

            if (amountToAdd > 0)
                pointScoreAudio.Play();

            if (score >= 3 && score < 6)
            {
                basketballHoop.GetComponent<BasketballHoop>().SetDifficulty(1);
            } else if (score >= 6)
            {
                basketballHoop.GetComponent<BasketballHoop>().SetDifficulty(2);
            } else if (score >= 9)
            {
                basketballHoop.GetComponent<BasketballHoop>().SetDifficulty(3);
            }
        }
    }

    private void CountTime()
    {
        timer -= Time.deltaTime;
        timerText.text = "Time Remaining: " + Mathf.RoundToInt(timer);

        if (timer <= 5 && !shotClockIsActive)
        {
            shotClockAudio.Play();
            shotClockIsActive = true;
        }
        if (timer <= 0)
        {
            EndGame();
        }
    }

    private void SetupUI()
    {
        titleScreen.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartPreGame()
    {
        StartCoroutine(PreGameCountdown());
    }

    IEnumerator PreGameCountdown()
    {
        gameMusic.Play();
        menuMusic.Stop();
        HideCursor();
        SetupUI();
        gameCamera.gameObject.SetActive(true);

        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        countdownText.color = Color.red;
        yield return new WaitForSeconds(1);

        countdownText.text = "2";
        countdownText.color = Color.yellow;
        yield return new WaitForSeconds(1);

        countdownText.text = "1";
        countdownText.color = Color.green;
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
        StartGame();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
