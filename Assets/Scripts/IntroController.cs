using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour {

    [System.Serializable]
    public class DialogueSection
    {
        [TextArea(3,5)]
        public string text;
    }

    public Text targetText;
    public DialogueSection[] dialogueSections;
    Queue<DialogueSection> dialogueQueue;

    float currentLetterTime = 0f;
    int currentLetterIndex = 0;
    DialogueSection currentDialog;

    public void StartDialogue()
    {
        if (dialogueSections.Length <= 0) return;

        dialogueQueue = new Queue<DialogueSection>();
        foreach (DialogueSection ds in dialogueSections)
        {
            dialogueQueue.Enqueue(ds);
        }
        //if (!targetText.IsActive()) return;
        TypeSentence(dialogueQueue.Dequeue());

    }

    void TypeSentence(DialogueSection ds)
    {
        targetText.text = "";
        currentDialog = ds;
        currentLetterIndex = 0;
    }

    void FinishDialogues()
    {
        GameController.gc.FinishIntro();
    }

    void Start()
    {
        StartDialogue();
    }
	
	// Update is called once per frame
    void Update()
    {
        currentLetterTime += Time.deltaTime;
        if (currentLetterTime > 0.04f)
        {
            currentLetterTime = 0f;
            if (currentLetterIndex < currentDialog.text.Length)
                targetText.text += currentDialog.text[currentLetterIndex++].ToString();
        }

        if (Input.GetMouseButtonDown(0)) {
            if (dialogueQueue.Count <= 0)
            {
                FinishDialogues();
                return;
            }
            TypeSentence(dialogueQueue.Dequeue());
        }

    }

}
