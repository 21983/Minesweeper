using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CellGrid : MonoBehaviour {

    public GameObject test;
    public int rowLength = 10;
    public int columnHeight = 10;
    private int bombCount;
    private int uncoveredCounter;
    private bool lost;

    public Cell[,] grid;

    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject textObject;
    private UnityEngine.UI.Text text;

    void Start()
    {

        canvas.SetActive(false);

        grid = new Cell[rowLength, columnHeight];

        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < columnHeight; j++)
            {
                grid[i, j] = Instantiate(test, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity).AddComponent<Cell>();
                grid[i, j].isBomb = Random.value <= 0.17;
                grid[i, j].x = i;
                grid[i, j].y = j;
            }
        }

        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < columnHeight; j++)
            {
                if (grid[i, j].isBomb)
                    bombCount++;
            }
        }

        Camera.main.transform.position = new Vector3(rowLength / 2, columnHeight / 2, -10);
        Camera.main.orthographicSize = columnHeight / 2;

        text = textObject.GetComponent<UnityEngine.UI.Text>();

    }

    void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(ray, out info, Mathf.Infinity))
            {
                var cell = info.collider.GetComponent<Cell>();
                if (!cell.isRevealed)
                {
                    cell.Flag();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit info;
            if (Physics.Raycast(ray, out info, Mathf.Infinity))
            {
                var cell = info.collider.GetComponent<Cell>();
                if (!cell.isFlagged)
                {
                    RevealBlock(cell.x, cell.y);

                    if (grid[cell.x, cell.y].isBomb)
                    {
                        lost = true;
                        GameOver();
                    }

                    if (rowLength * columnHeight <= uncoveredCounter + bombCount)
                    {
                        Win();
                    }


                }
            }

        }
    }


    private void RevealBlock(int x, int y)
    {
        if (x < 0 || x >= rowLength || y < 0 || y >= columnHeight) return;

        if (!grid[x, y].isRevealed)
        {
            MineAt(x + 1, y, x, y);
            MineAt(x + 1, y + 1, x, y);
            MineAt(x, y + 1, x, y);
            MineAt(x + 1, y - 1, x, y);
            MineAt(x - 1, y + 1, x, y);
            MineAt(x - 1, y, x, y);
            MineAt(x, y - 1, x, y);
            MineAt(x - 1, y - 1, x, y);

            if(!lost)
            uncoveredCounter++;

            grid[x, y].DisplayText();

            if (grid[x, y].bombsNearby <= 0)
            {
                RevealBlocksAround(x, y);
            }
        }
    }

    private void RevealBlocksAround(int x, int y)
    {
        RevealBlock(x + 1, y);
        RevealBlock(x + 1, y + 1);
        RevealBlock(x, y + 1);
        RevealBlock(x + 1, y - 1);
        RevealBlock(x - 1, y + 1);
        RevealBlock(x - 1, y);
        RevealBlock(x, y - 1);
        RevealBlock(x - 1, y - 1);
    }

    private void MineAt(int x, int y, int i, int j)
    {
        if (x < 0 || x >= rowLength || y < 0 || y >= columnHeight) return;
        
        if (grid[x, y].isBomb)
        {
            grid[i, j].bombsNearby += 1;
        }
    }

    private void GameOver()
    {
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < columnHeight; j++)
            {
                RevealBlock(i,j);
            }
        }
        text.text = "You lose noob";
        canvas.SetActive(true);
    }

    private void Win()
    {
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < columnHeight; j++)
            {
                RevealBlock(i, j);
            }
        }
        text.text = "Je hebt gewonnen, eh Kaboem";
        canvas.SetActive(true);
    }

}
