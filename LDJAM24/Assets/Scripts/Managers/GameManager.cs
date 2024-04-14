using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float score;
    public float multiplier = 1.5f;

    // Timer variables
    public float maxTimerDuration = 10f;
    private float currentTimer;
    private Coroutine timerCoroutine;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

   public void AddScoreWithMultiplier(float amount)
    { 
        if (multiplier == 0)
        {
            score += amount;
        }
        else
        {
            score += amount * multiplier;
        }
    }

    public void EnemyKilled()
    {
        // Reset the timer
        if(currentTimer <= 0)
        {
            currentTimer = maxTimerDuration;
            multiplier += 0.5f;

            // Start or restart the timer coroutine
            if (timerCoroutine != null)
                StopCoroutine(timerCoroutine);
            timerCoroutine = StartCoroutine(UpdateTimer());

        }
        else
        {
            currentTimer += maxTimerDuration;
        }


        // Add score with multiplier
        AddScoreWithMultiplier(1);
    }
    public void ResetScore()
    {
        score = 0;
    }

    public IEnumerator UpdateTimer()
    {
        while (true)
        {
            yield return null;
            currentTimer -= Time.deltaTime;

            if (currentTimer <= 0)
            {
                // Reset multiplier
                multiplier = 1f;
                yield break;
            }
        }
    }
}
