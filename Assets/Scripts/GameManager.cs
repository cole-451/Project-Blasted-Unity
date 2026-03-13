using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
	static GameManager instance;

    [SerializeField] GameObject titlePanel;
    public static GameManager Instance 
	{ 
		get 
		{
			return instance; 
		} 
	}

	//[SerializeField] TMP_Text scoreText;

	private int score;

    private void Start()
    {
        Time.timeScale = 0.0f;

    }

    void Awake()
	{
		instance = this;
		//titlePanel.SetActive(true);
	}

    public void OnGameStart()
    {
        titlePanel.SetActive(false);
        print("I am here.");
        Time.timeScale = 1.0f;

    }

    void Update()
	{
		//scoreText.text = score.ToString("0000");
	}

	public void AddPoints(int points)
	{
		score += points;
	}
}
