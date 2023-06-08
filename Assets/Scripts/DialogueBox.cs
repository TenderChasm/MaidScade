using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public struct Monologue
    {
        public string Name;
        public List<string> Lines;
    }

    public float TimeoutBetweenCharacters = 0.1F;
    public Action DialogueDoneCallback;


    public List<Monologue> TextToShow { get; set; }

    public TextMeshProUGUI Name;
    public TextMeshProUGUI Message; 

    private int currentMonologue = 0;
    private int currentLineInMonologue = 0;

    public void StartShowing(Action dialogueDone = null)
    {
        if (dialogueDone != null)
            DialogueDoneCallback = dialogueDone;
        currentMonologue = 0;
        currentLineInMonologue = 0;
        Show();
    }

    private void Show()
    {
        Time.timeScale = 0;
        GameManager.Hr.IsInputLocked = true;
        StartCoroutine(ShowIndividualMonologueWithTyping());
    }

    private IEnumerator ShowIndividualMonologueWithTyping()
    {
        Name.text = TextToShow[currentMonologue].Name;

        string lineToShow = TextToShow[currentMonologue].Lines[currentLineInMonologue];
        for(int i = 0; i < lineToShow.Length; i++)
        {
            string growingString = lineToShow[0..(i+1)];
            Message.text = growingString;
            yield return new WaitForSecondsRealtime(TimeoutBetweenCharacters);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            if(currentLineInMonologue < TextToShow[currentMonologue].Lines.Count - 1)
            {
                currentLineInMonologue++;
                Show();
            }
            else
            {
                if(currentMonologue < TextToShow.Count - 1)
                {
                    currentMonologue++;
                    currentLineInMonologue = 0;
                    Show();
                }
                else
                {
                    Time.timeScale = 1;
                    GameManager.Hr.IsInputLocked = false;
                    DialogueDoneCallback?.Invoke();
                    GameManager.Hr.Dialogue.gameObject.SetActive(false);
                }
            }
        }
            
    }
}
