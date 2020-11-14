﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text ScoreText;
    public GameObject FloatingText;
    public List<int> Score = new List<int>();
    public List<int> HighScore = new List<int>();
    public AudioSource WoodDestruction;
    public AudioSource IceDestruction;
    public AudioSource PigDestroy;
    public AudioSource BirdDestroy;
    public AudioSource PigHit;
    public AudioSource LevelCleared;

    private bool _isLevelCleared;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (!_isLevelCleared && GameObject.FindGameObjectsWithTag("Pig").Length == 0)
        {
            _isLevelCleared = true;
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
}