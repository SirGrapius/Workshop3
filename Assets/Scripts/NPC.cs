using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private BoxCollider2D triggerBox;
    [SerializeField] private PlatformerMovement player;
    [SerializeField] private TextMeshPro interactIndicator;

    [SerializeField] public List<string> myDialogs;
    private int dialogIndex = 0;
    
    
    [SerializeField] DialogScript dialog;
    void Awake()
    {
        triggerBox = GetComponent<BoxCollider2D>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        interactIndicator = GetComponentInChildren<TextMeshPro>();
        player = null;
        dialog.gameObject.SetActive(false);
        interactIndicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player && player.inDialog)
        {
            dialog.text.text = myDialogs[dialogIndex];

            if (Keyboard.current[Key.E].wasPressedThisFrame)
            {
                dialogIndex++;
                if (dialogIndex >= myDialogs.Count - 1)
                {
                    EndDialogue();
                }
            }
        }
    }

    public void Talk(PlatformerMovement currentPlayer)
    {
        player = currentPlayer;
        if (player.transform.position.x > transform.position.x)
        {
            renderer.flipX = false;
        }
        else
        {
            renderer.flipX = true;
        }
        player.inDialog = true;
        dialog.gameObject.SetActive(true);
        dialog.text.text = myDialogs[dialogIndex];

    }

    private void EndDialogue()
    {
        dialog.gameObject.SetActive(false);
        player.inDialog = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            interactIndicator.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            interactIndicator.gameObject.SetActive(false);
        }
    }
}
