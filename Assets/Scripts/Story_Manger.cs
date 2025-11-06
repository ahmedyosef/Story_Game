using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;
using Sirenix.OdinInspector; // 👈 Odin Inspector

public class Story_Manager : MonoBehaviour
{
    [Header("Story Settings")]
    public List<UnityEvent2> stepsEvent;
    public List<StoryStep> steps = new List<StoryStep>();
    private int currentStep = 0;

    [Header("Global Delay Control")]
    public bool useGlobalDelay = true;        // ✅ Turn all delays on/off
    public float globalDelayMultiplier = 1f;  // Optional multiplier (e.g., 0.5 for half-speed delays)

    [Header("Video System")]
    public VideoPlayer videoPlayer;

    [Header("UI")]
    public Text stepText; // Assign a UI Text or TMP_Text object in the Inspector

    private bool waitingForVideo;
    private float vpStartTime;
    private float vpLength;

    private void Start()
    {
        //if (videoPlayer != null)
        //    videoPlayer.loopPointReached += OnVideoFinished;

        StartCoroutine(PlayStep(currentStep));
    }

    private void Update()
    {
        if (waitingForVideo)
        {
            if (Time.time - vpStartTime >= vpLength)
            {
                waitingForVideo = false;
                NextStep();
            }
        }
    }

    public IEnumerator PlayStep(int stepIndex)
    {
        if (stepIndex < 0 || stepIndex >= steps.Count)
        {
            Debug.LogWarning("Step index out of range!");
            yield break;
        }

        StoryStep step = steps[stepIndex];
        //stepsEvent[stepIndex].Invoke();

        //// 📝 Update step text
        if (stepText != null)
            stepText.text = step.stepName;

        // 🕓 Handle delay based on bools
        if (useGlobalDelay && step.useDelay && step.delayBefore > 0)
        {
            float actualDelay = step.delayBefore * globalDelayMultiplier;
            Debug.Log($"⏳ Waiting {actualDelay} seconds before step {stepIndex}");
            yield return new WaitForSeconds(actualDelay);
        }

        // ▶️ Trigger step event
        Debug.Log($"▶️ Starting step {stepIndex}: {step.stepName}");
        step.onStepStart.Invoke();
    }

    public void NextStep()
    {
        currentStep++;
        if (currentStep < steps.Count)
        {
            StartCoroutine(PlayStep(currentStep));
        }
        else
        {
            //Debug.Log("✅ Story complete!");
            if (stepText != null)
                stepText.text = "✅ Story Complete!";
        }
    }
    public void NextStepNumber(int stepNumber)
    {
        if (stepNumber < 0 || stepNumber >= steps.Count)
        {
            Debug.LogWarning("⚠️ Step number out of range: " + stepNumber);
            return;
        }

        // Set the current step correctly
        currentStep = stepNumber;

        Debug.Log($"➡️ Jumping to step {stepNumber}: {steps[stepNumber].stepName}");
        StartCoroutine(PlayStep(currentStep));
    }


    public void PreviousStep()
    {
        currentStep--;
        if (currentStep >= 0)
        {
            StartCoroutine(PlayStep(currentStep));
        }
    }

    public IEnumerator PlayVideoAndWait()
    {
        if (videoPlayer == null)
        {
            Debug.LogWarning("No video player assigned!");
            yield break;
        }

        videoPlayer.Play();
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        NextStep();
    }

    public void OnVideoFinished()
    {
        Debug.Log("🎬 Video finished. Moving to next step...");
        vpStartTime = Time.time;
        vpLength = (float)videoPlayer.length + 1f;
        waitingForVideo = true;
    }
}
[System.Serializable]
public class StoryStep
{
    public string stepName = "Step";          // Step title or description
    public bool useDelay = false;              // ✅ Should this step wait before running?
    public float delayBefore = 0f;            // How long to wait before executing this step
    public UnityEvent onStepStart;            // What happens at this step
}