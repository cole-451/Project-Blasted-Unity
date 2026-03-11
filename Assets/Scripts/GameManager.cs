using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
	static GameManager instance;
	public static GameManager Instance 
	{ 
		get 
		{
			return instance; 
		} 
	}

	//[SerializeField] TMP_Text scoreText;

	private int score;

	void Awake()
	{
		instance = this;
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
