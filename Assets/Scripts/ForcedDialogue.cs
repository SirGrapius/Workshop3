using System.Collections.Generic;
using TMPro;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForcedDialogue : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private bool triggered;

    [SerializeField] private PlatformerMovement player;
    [SerializeField] private DialogScript dialog;

    [SerializeField] public List<string> myDialogs;
    private int dialogIndex = 0;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        dialog = GameObject.FindGameObjectWithTag("DialogBox").GetComponent<DialogScript>();
    }


    void Update()
    {
        if (player && player.inDialog)
        {
            dialog.text.text = myDialogs[dialogIndex];

            if (Keyboard.current[Key.E].wasPressedThisFrame)
            {
                dialogIndex++;
                if (dialogIndex > myDialogs.Count - 1)
                {
                    EndDialogue();
                }
            }
        }
    }

    public void Talk(PlatformerMovement currentPlayer)
    {
        player = currentPlayer;
        player.inDialog = true;
        dialog.gameObject.SetActive(true);
        dialog.text.text = myDialogs[dialogIndex];

    }

    private void EndDialogue()
    {
        dialog.gameObject.SetActive(false);
        dialogIndex = 0;
        player.inDialog = false;
        StartCoroutine(player.DialogCooldown());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        { 
            triggered = true;
            Talk(collision.GetComponent<PlatformerMovement>());
        }
    }
}
