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

    public void StartDialogue()
    {
        if (dialogueSections.Length <= 0) return;

        dialogueQueue = new Queue<DialogueSection>();
        foreach (DialogueSection ds in dialogueSections)
        {
            dialogueQueue.Enqueue(ds);
        }
        StopAllCoroutines();
        if (!targetText.IsActive()) return;
        StartCoroutine(TypeSentence(dialogueQueue.Dequeue()));

    }

    IEnumerator TypeSentence(DialogueSection ds)
    {
        targetText.text = "";
        foreach (char letter in ds.text.ToCharArray())
        {
            targetText.text += letter.ToString();
            yield return new WaitForSeconds(0.04f);
        }
        
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
        if (Input.GetMouseButtonDown(0)) {
            if (dialogueQueue.Count <= 0)
            {
                FinishDialogues();
                return;
            }
            StopAllCoroutines();
            StartCoroutine(TypeSentence(dialogueQueue.Dequeue()));
        }

    }

}
