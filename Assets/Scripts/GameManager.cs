using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text ScoreText;
    public GameObject FloatingText;
    public GameObject SlingshotBird;
    public GameObject StillBird;
    public Slingshot Slingshot;
    public int RemainingBirds = 3;
    public bool IsLevelCleared;
    public List<int> Score = new List<int>();
    public List<int> HighScore = new List<int>();
    public AudioSource WoodDestruction;
    public AudioSource IceDestruction;
    public AudioSource PigDestroy;
    public AudioSource BirdDestroy;
    public AudioSource PigHit;
    public AudioSource LevelCleared;
    public AudioSource LevelFailed;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SetNewBird();
    }

    void Update()
    {
        if (!IsLevelCleared && GameObject.FindGameObjectsWithTag("Pig").Length == 0)
        {
            IsLevelCleared = true;
            LevelCleared.Play();
        }
    }

    public void AddScore(int amount, Vector3 position, Color textColor)
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        if (Score.Count <= level)
        {
            Score.Add(amount);
        }
        else
        {
            Score[level] += amount;
        }
        ScoreText.text = Score[level].ToString();
        GameObject floatingTextObj = Instantiate(FloatingText, position, Quaternion.identity);
        FloatingText floatingText = floatingTextObj.GetComponent<FloatingText>();
        floatingText.UpdateText(amount.ToString(), textColor);
    }

    public void SetNewBird()
    {
        RemainingBirds--;
        if (RemainingBirds >= 0)
        {
            GameObject bird = Instantiate(SlingshotBird, new Vector3(Slingshot.transform.position.x - 0.08f, Slingshot.transform.position.y + 3.82f, Slingshot.transform.position.z - 0.29f), Quaternion.identity);
            Slingshot.Bird = bird;
            Camera.main.GetComponent<MainCamera>().Bird = bird;

            foreach (StillBird stillBird in FindObjectsOfType<StillBird>())
            {
                Destroy(stillBird.gameObject);
            }

            if (RemainingBirds > 0)
            {
                for (int i = 0; i < RemainingBirds; i++)
                {
                    GameObject stillBird = Instantiate(StillBird, new Vector3(0, 0, 0), Quaternion.identity);
                    stillBird.transform.Find("Bird Body").transform.position = new Vector3(-2.5f * (i + 1), 0, -3.19f);
                    if (i % 2 == 0)
                    {
                        stillBird.GetComponent<StillBird>().WaitForSeconds = 0.45f;
                    }
                }
            }
        }

        if (IsLevelCleared && RemainingBirds >= 0)
        {
            StartCoroutine(AddFinalScores());
        }
        else if (RemainingBirds < 0)
        {
            LevelFailed.Play();
        }
    }

    IEnumerator AddFinalScores()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (StillBird stillBird in FindObjectsOfType<StillBird>())
        {
            AddScore(10000, stillBird.transform.Find("Bird Body").transform.position, Color.red);
        }
        foreach (Bird bird in FindObjectsOfType<Bird>())
        {
            AddScore(10000, bird.transform.position, Color.red);
        }
    }
}