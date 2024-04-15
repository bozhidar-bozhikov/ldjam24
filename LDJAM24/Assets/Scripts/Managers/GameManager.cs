using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score;
    public int multiplier = 1;

    // Timer variables
    public float maxTimerDuration = 10f;
    private float currentTimer;
    private Coroutine timerCoroutine;
    public TextmodeSentence scoreText;
    public TextmodeSentence multText;
    public RectTransform timerFill;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ResetScore();
    }

    public void AddScoreWithMultiplier(int amount)
    { 
        if (multiplier == 0)
        {
            score += amount;
            scoreText.DisplayString(score + "");
        }
        else
        {
            score += amount * multiplier;
            scoreText.DisplayString(score + "");
        }
    }

    public void EnemyKilled()
    {
        // Reset the timer
        if(currentTimer <= 0)
        {
            currentTimer = maxTimerDuration;
            multiplier += 1;
            multText.DisplayString(multiplier + "");

            StartCoroutine(UpdateTimer());
        }
        else
        {
            currentTimer = maxTimerDuration;
        }

        // Add score with multiplier
        AddScoreWithMultiplier(1);
    }
    public void ResetScore()
    {
        score = 0;
        scoreText.DisplayString(0 + "");
        multiplier = 1;
        multText.DisplayString(1 + "");
    }

    public IEnumerator UpdateTimer()
    {
        while (currentTimer > 0)
        {
            yield return null;
            currentTimer -= Time.deltaTime;

            float sizeX = currentTimer / maxTimerDuration * 84;
            timerFill.sizeDelta = new Vector2(Mathf.Clamp(sizeX, 0, 40), 4);

            if (currentTimer <= 0)
            {
                // Reset multiplier
                multiplier = 1;
                multText.DisplayString(1 + "");

                yield break;
            }
        }
    }
}
