using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    public bool isBomb;
    public bool isRevealed = false;
    public bool isFlagged = false;
    public int x = 0;
    public int y = 0;
    public TextMesh text;
    public int bombsNearby = 0;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        text = transform.GetChild(0).GetComponent<TextMesh>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Flag()
    {
        isFlagged = !isFlagged;

        if (isFlagged)
            text.text = "F";
        if (!isFlagged)
            text.text = "";
    }


    public void DisplayText()
    {
        if (!isRevealed)
        {
            isRevealed = true;
            if (isBomb)
            {
                text.text = "B";
            }
            else
            {
                spriteRenderer.color = Color.white;
                if (bombsNearby > 0)
                    text.text =  "" + bombsNearby;
            }
        }
    }

}
