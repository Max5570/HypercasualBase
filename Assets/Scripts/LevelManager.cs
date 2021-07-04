using System;
using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : UnitySingletonPersistent<LevelManager>
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _winPanel;

    private bool _gameOver = false;
    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_gameOver)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                Time.timeScale = 1;
                _menuPanel.SetActive(false);
            }
        }
    }

    public void GameOver()
    {
        _gameOver = true;
        _losePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Finish()
    {
        _gameOver = true;
        _winPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
