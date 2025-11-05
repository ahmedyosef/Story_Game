using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
public class Drobber : MonoBehaviour
{
    [Space]
    [Header("Audio")]
    [HideInInspector] public  AudioSource correct_Answer;
    [HideInInspector] public AudioSource wrong_Answer;
    [Space]
    [Header("Game Elements")]
    [HideInInspector] public  List<GameObject> main_Objects = new List<GameObject>();
    [Space]
    [HideInInspector] public  List<GameObject> shadow_Objects = new List<GameObject>();
    [Space]
    [HideInInspector] public List<Vector2> initial_Positions = new List<Vector2>(); [Space]
    [Header("Score")]
    [HideInInspector] public Score_System score_System;

    private void Start()
    {
        score_System.UpdateScoreText();
        // Now read the actual anchored positions
        foreach (var obj in main_Objects)
        {
            initial_Positions.Add(obj.GetComponent<RectTransform>().anchoredPosition);
        }
    }
    public void OnDrop(GameObject droppedObject, Vector2 pointerPosition)
    {
        ImageID droppedId = droppedObject.GetComponent<ImageID>();
        if (droppedId == null)
            return; // Invalid draggable — ignore

        // Find the closest shadow within max distance
        GameObject bestMatch = null;
        float closestDist = 50f; // Max allowed distance in pixels

        foreach (var shadow in shadow_Objects)
        {
            RectTransform shadowRT = shadow.GetComponent<RectTransform>();
            float dist = Vector2.Distance(pointerPosition, shadowRT.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                bestMatch = shadow;
            }
        }

        if (bestMatch != null)
        {
            ImageID shadowId = bestMatch.GetComponent<ImageID>();

            if (shadowId != null && shadowId.id == droppedId.id)
            {
                // ✅ CORRECT MATCH
                droppedObject.GetComponent<RectTransform>().position = bestMatch.GetComponent<RectTransform>().position;
                droppedObject.GetComponent<Draggable>().enabled = false;
                correct_Answer.Play();
                score_System.AddAnswer(true);
                return;
            }
            else
            {
                // ❌ WRONG MATCH (either shadowId is null or IDs don't match)
                ResetToInitialPosition(droppedObject);
                wrong_Answer.Play();
                score_System.AddAnswer(false);
                return;
            }
        }
        else
        {
            // 🚫 DROPPED TOO FAR FROM ANY SHADOW (no valid target)
            ResetToInitialPosition(droppedObject);
            wrong_Answer.Play();
        }
    }
    // Helper method to avoid code duplication
    private void ResetToInitialPosition(GameObject obj)
    {
        int index = main_Objects.IndexOf(obj);
        if (index >= 0 && index < initial_Positions.Count)
        {
            obj.GetComponent<RectTransform>().anchoredPosition = initial_Positions[index];
        }
    }
}
