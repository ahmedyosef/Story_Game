using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using System;
using Unity.VisualScripting;

public class True_Or_False_GameManger : MonoBehaviour
{
    [Header("Sadow Game Data")]
    [SerializeField] private True_Or_False_Data gameData; // Your ScriptableObject
    [Header("UI Setup")]
    [SerializeField] private GameObject game_Image; // Prefab image to instantiate
    [SerializeField] private GameObject mainObjects_Contener;
    [ReadOnly][SerializeField] private List<GameObject> quizes;
    Button2[] buttons;
   [Space]
    [Header("Audio")]
    [SerializeField] private AudioSource correct_Answer;
    [SerializeField] private AudioSource wrong_Answer;

    [Space]
    [Header("Audio")]
    [SerializeField] Score_System score_System;
    int Awnser_id;

    private void Awake()
    {
        score_System.game_Element.Clear();
        score_System.totalQuestions = gameData.Quize_Text.Count;
        score_System.UpdateScoreText();
    }

    void Start()
    {
        for (int i = 0; i < gameData.Quize_Text.Count; i++)
        {
            int id = gameData.imge_ID[i];
            string text = gameData.Quize_Text[i];
            GameObject mainObj = Instantiate(game_Image, mainObjects_Contener.transform);
            mainObj.gameObject.GetComponentInChildren<TMP_Text>().text = text;
            buttons = mainObj.GetComponentsInChildren<Button2>();
            buttons[0].onClick2.AddListener((PointerEventData) => Correct_Anser(id));
            buttons[1].onClick2.AddListener((PointerEventData) => Wrong_Anser(id));
            quizes.Add(mainObj);
            
        }
        for (int j = 1; j <= quizes.Count; j++)
        {
            quizes[j].SetActive(false);
        }
    }
    public void Correct_Anser(int arg0)
    {
        Debug.Log(1);
        chikeAnsewr(arg0, true);
    }
    public void Wrong_Anser(int arg0) 
    {
        Debug.Log(0);
        chikeAnsewr(arg0,false);
    }
    public void chikeAnsewr(int id ,bool chick)
    {
        
        bool ischecked = chick;
        if (gameData.chick_Correct_Or_Not[id] == true && ischecked==true)
        {
            correct_Answer.Play();
            score_System.AddAnswer(true);
            quizes[id].GetComponent<Image>().color = Color.green;
            buttons = quizes[id].GetComponentsInChildren<Button2>();
            buttons[0].interactable = false;
            buttons[1].interactable = false;
            Invoke("AwnserEffeect", 2);
        }
        else if (gameData.chick_Correct_Or_Not[id] == false && ischecked == false)
        {
            correct_Answer.Play();
            score_System.AddAnswer(true);
            quizes[id].GetComponent<Image>().color = Color.green;
            buttons = quizes[id].GetComponentsInChildren<Button2>();
            buttons[0].interactable = false;
            buttons[1].interactable = false;
            Invoke("AwnserEffeect",0.5f);
        }
        else 
        {
            wrong_Answer.Play();
            score_System.AddAnswer(false);
            buttons = quizes[Awnser_id].GetComponentsInChildren<Button2>();
            buttons[0].interactable = false;
            buttons[1].interactable = false;
            Invoke("buttonEffeect",0.5f);
            quizes[id].GetComponent<Image>().color = Color.red;
        }

        Awnser_id=id;
    }
    public void AwnserEffeect()
    {
        if (Awnser_id < gameData.Quize_Text.Count)
        {
            quizes[Awnser_id].SetActive(false);
            quizes[Awnser_id + 1].SetActive(true);
        }
        else if (Awnser_id == gameData.Quize_Text.Count)
        {
            quizes[Awnser_id].SetActive(false);
        }

    }
    public void buttonEffeect()
    {
        buttons[0].interactable = true;
        buttons[1].interactable = true;
    }

}