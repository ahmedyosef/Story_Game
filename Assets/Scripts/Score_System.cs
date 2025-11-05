using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using RTLTMPro;

public class Score_System : MonoBehaviour
{
    [Header("Score Tracking")]
    public int correct_Answer;
    public int wrong_Answer;
    public int totalQuestions; // Total number of questions/interactions expected

    public List<GameObject> zarzor;
    public GameObject masseg;
    public List<GameObject> game_Element;

    [ReadOnly]
    [SerializeField] private int percentage;

    [Header("UI")]
    [SerializeField] private RTLTextMeshPro scoreText;
    [SerializeField] private RTLTextMeshPro resalt;

    // Optional: Event when game ends
    public System.Action<int> OnGameCompleted;

    public void AddAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            correct_Answer++;
        }
        else
        {
            wrong_Answer++;
        }

        UpdateScoreText();

        // Check if all questions have been answered
        if (correct_Answer + wrong_Answer >= totalQuestions)
        {
            CalculateResult();
        }
    }

    public void UpdateScoreText()
    {
        // Optional: Show "X / Y" instead of just X

        scoreText.text = correct_Answer.ToString()+"/"+totalQuestions; // or $"{correct_Answer} / {totalQuestions}"
    }

    void CalculateResult()
    {
        if (totalQuestions <= 0)
        {
            Debug.LogError("totalQuestions must be greater than 0!");
            percentage = 0;
            OnGameCompleted?.Invoke(percentage);
            return;
        }
        int result = correct_Answer-wrong_Answer;
        // ✅ CORRECT: Percentage = (correct / total) * 100
        percentage = Mathf.RoundToInt((float)result / totalQuestions * 100);
        if (correct_Answer == totalQuestions) 
        {
            resalt.text = "Correct \n answer " + percentage.ToString() + "%";
            foreach (GameObject element in game_Element)
            {
                element.SetActive(false);
            }
            // Log result
            if (percentage <= 100)
            {
                Debug.Log("Perfect! 100% score!");
                zarzor[0].SetActive(true);
                masseg.SetActive(true);
            }
            else if (percentage <= 75)
            {
                Debug.Log($"Great job! Score: {percentage}%");
                masseg.SetActive(true);
                zarzor[1].SetActive(true);
            }
            else if (percentage <= 50)
            {
                Debug.Log($"Good effort! Score: {percentage}%");
                masseg.SetActive(true);
                zarzor[2].SetActive(true);
            }
            OnGameCompleted?.Invoke(percentage);
            masseg.GetComponentsInChildren<TMP_Text>()[4].text = resalt.text.ToString();
            PostMasseg();
        }
    }

    // Optional: Reset score (useful for replay)
    public void ResetScore()
    {
        correct_Answer = 0;
        wrong_Answer = 0;
        percentage = 0;
        UpdateScoreText();
    }
    public void PostMasseg()
    {

       Application.ExternalCall("onUnityPlayClicked"+1,"game"+1, "percentage"+ resalt.text);
        
    }
}