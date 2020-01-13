using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecisionWindowControl : MonoBehaviour
{
    public GameObject eventWindow;
    public TextMeshProUGUI title, decisionText;
    public TextMeshProUGUI[] answersTexts;
    public GameObject[] answersButtons;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawDecision(Decision decision, List<Answer> answersPack) //отрисовка решения и ответов к нему
    {
        //отрисовка заголовка и текста
        title.text = decision.name;
        decisionText.text = decision.text;

        //отрисовка ответов
        HideAnswers();
        int counter = 0;
        foreach (Answer answer in answersPack)
        {
            answersTexts[counter].text = answer.answer_text;
            answersButtons[counter].SetActive(true);
            counter++;
        }
        eventWindow.SetActive(true);
    }

    public void CloseWindow()
    {
        HideAnswers();
        eventWindow.SetActive(false);
    }

    void HideAnswers() //скрытие ответов к решению
    {
        foreach (GameObject answerButton in answersButtons)
        {
            answerButton.SetActive(false);
        }
    }
}
