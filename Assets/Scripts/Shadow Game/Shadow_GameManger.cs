using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shadow_GameManager : MonoBehaviour
{
    [Header("Sadow Game Data")]
    [SerializeField] private SadowGame_Data gameData; // Your ScriptableObject

    [Header("UI Setup")]
    [SerializeField] private Image game_Image; // Prefab image to instantiate
    [SerializeField] private GameObject mainObjects_Contener;
    [Space]
    [SerializeField] private Image point_Image; // Prefab image to instantiate
    [SerializeField] private GameObject shadowObjects_Contener;
    [Space]
    [ReadOnly][SerializeField] private Drobber drobber;

    [Space]
    [Header("Audio")]
    [SerializeField] private AudioSource correct_Answer;
    [SerializeField] private AudioSource wrong_Answer;

    [Space]
    [Header("Audio")]
    [SerializeField] Score_System score_System;

    private void Awake()
    {
        drobber = gameObject.GetComponent<Drobber>();
        drobber.score_System = score_System;
        drobber.correct_Answer = correct_Answer;
        drobber.wrong_Answer = wrong_Answer;
        score_System.game_Element.Clear();
        score_System.totalQuestions = gameData.game_Imge.Count;
        score_System.UpdateScoreText();
        score_System.game_Element.Add(shadowObjects_Contener);
        score_System.game_Element.Add(mainObjects_Contener);

        if (!gameData || gameData.game_Imge == null || gameData.imge_ID == null)
        {
            Debug.LogError("Game_Data is missing or not set up properly!");
            return;
        }

        if (gameData.game_Imge.Count != gameData.imge_ID.Count)
        {
            Debug.LogError("Sprite list and ID list must have the same length!");
            return;
        }

        // Temp list to hold shadows before shuffling
        List<GameObject> tempImge = new List<GameObject>();

        for (int i = 0; i < gameData.game_Imge.Count; i++)
        {
            Sprite sprite = gameData.game_Imge[i];
            int id = gameData.imge_ID[i];

            // Main (draggable)
            Image mainImg = Instantiate(game_Image, mainObjects_Contener.transform);
            mainImg.sprite = sprite;
            mainImg.color = Color.white;
            mainImg.gameObject.AddComponent<ImageID>().id = id;
            drobber.main_Objects.Add(mainImg.gameObject);

            // Shadow (target) — add to temp list first
            Image shadowImg = Instantiate(game_Image, Vector3.zero, Quaternion.identity); // Don't parent yet
            shadowImg.sprite = sprite;
            shadowImg.color = Color.black;
            shadowImg.color = new Color(0.0f , 0.0f, 0.0f, 0.5f);
            shadowImg.gameObject.GetComponent<Draggable>().enabled = false;
            shadowImg.gameObject.AddComponent<ImageID>().id = id;
            //shadow_Objects.Add(shadowImg.gameObject);
            tempImge.Add(shadowImg.gameObject);
        }

        // ✅ SHUFFLE the shadow objects
        ShuffleList(tempImge);

        // Now parent them to the container (in shuffled order)
        foreach (var shadow in tempImge)
        {
            shadow.transform.SetParent(shadowObjects_Contener.transform, false);
            drobber.shadow_Objects.Add(shadow);
        }
    }

    void Start()
    {
        // Disable layout groups so positions don't change during gameplay
        var mainLayout = mainObjects_Contener.GetComponent<GridLayoutGroup>();
        var shadowLayout = shadowObjects_Contener.GetComponent<GridLayoutGroup>();

        // Force Unity to calculate layout NOW
        Canvas.ForceUpdateCanvases();

        // Now disable layout so dragging isn't overridden
        if (mainLayout != null) mainLayout.enabled = false;
        if (shadowLayout != null) shadowLayout.enabled = false;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[randomIndex];
            list[randomIndex] = list[i];
            list[i] = temp;
        }
    }
}