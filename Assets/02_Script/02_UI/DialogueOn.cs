using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOn : MonoBehaviour
{
    public Dialogue[] dialogue;

    private DialogueManager theDM;
    private bool isOn;

    void Awake()
    {
        theDM = FindObjectOfType<DialogueManager>();
        isOn = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOn)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Animator>().SetLayerWeight(1, 0);
                isOn = true;
                GameController.instance.CanMove = false;
                GameController.instance.CanShot = false;
                theDM.ShowDialogue(dialogue[GameController.instance.StageLevel - 1]);
            }
        }
    }
}
