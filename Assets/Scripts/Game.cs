using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public GameObject[] groups;
    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public Transform[,] grid = new Transform[gridWidth, gridHeight];

    public int scoringLineOne = 100;
    public int scoringLineTwo = 200;
    public int scoringLineThree = 300;
    public int scoringLineFour = 400;

    public Text hud_score;
    public static int currentScore = 0;

    private int numberOfRowsDelete = 0;

    // Use this for initialization
    void Start () {

        spawnNext();
    }
	
    void Update()
    {
        UpdateScore();
        UpdateUI();
    }

    public void UpdateGrid(MovePiece tetris)
    {
        for(int y = 0; y < gridHeight; y++)
        {
            for(int x = 0; x < gridWidth; x++)
            {
                if(grid[x,y] != null)
                {
                    if (grid[x,y].parent == tetris.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }

        foreach(Transform piece in tetris.transform)
        {
            Vector3 pos = Round(piece.position);

            if(pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = piece;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector3 pos)
    {
        if(pos.y > gridHeight -1)
        {
            return null;
        } else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    public bool CheckIsInsideGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector3 Round(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
    }

    public void spawnNext()
    {
        // Random Index
        int i = Random.Range(0, groups.Length);

        Instantiate(groups[i],
                    new Vector3(5f, 21f, 0),
                    Quaternion.identity);
    }

    #region Game Over

    public bool CheckIsAboveGrid(MovePiece tetris)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            foreach(Transform piece in tetris.transform)
            {
                Vector3 pos = Round(piece.position);

                if(pos.y > gridHeight - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    #endregion

    #region Gestion du score

    public void UpdateUI()
    {
        hud_score.text = currentScore.ToString();
    }

    public void UpdateScore()
    {
        if(numberOfRowsDelete > 0)
        {
            if (numberOfRowsDelete == 1)
            {
                currentScore += scoringLineOne;
            } else if (numberOfRowsDelete == 2)
            {
                currentScore += scoringLineTwo;
            } else if (numberOfRowsDelete == 3)
            {
                currentScore += scoringLineThree;
            } else  if (numberOfRowsDelete == 4)
            {
                currentScore += scoringLineFour;
            }

            numberOfRowsDelete = 0;
        }
    }

    #endregion

    #region Supprimer les lignes pleines

    public bool IsFullRow(int y)
    {
        for(int x =0; x < gridWidth; ++x)
        {
            if (grid[x,y] == null)
            {
                return false;
            }
        }

        numberOfRowsDelete++;

        return true;
    }

    public void DeleteTetris(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            Debug.Log("Game object destroy x " + x + "et y " +y + " " +  grid[x, y].gameObject);
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x < gridWidth; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y -1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position = Vector3.down;
            }
        }
    }

    public void MoveRowsDown(int y)
    {
        for (int i = y; i < gridHeight; ++i)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow()
    {
        for (int y = 0; y < gridHeight; ++y)
        {
            if(IsFullRow(y))
            {
                Debug.Log("Je supprime la ligne" + y);
                DeleteTetris(y);
                MoveRowsDown(y + 1);

                --y;
            }
        }
    }
    #endregion
}
