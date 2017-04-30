using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : MonoBehaviour {

    float fall = 0;
    public float speedFall = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    public int individualScore = 100;

    private float individualScoreTime;

    void Update()
    {
        ChekInput();

        UpdateIndividualScore();
    }

    void UpdateIndividualScore()
    {
        if(individualScoreTime < 1)
        {
            individualScoreTime += Time.deltaTime;
        } else
        {
            individualScoreTime = 0;
            individualScore = Mathf.Max(individualScore - 10, 0);
        }
    }

    public void ChekInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            this.transform.position += new Vector3(1, 0, 0);

            if (!CheckIsValidPosition())
                this.transform.position += new Vector3(-1, 0, 0);
            else
                FindObjectOfType<Game>().UpdateGrid(this);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            this.transform.position += new Vector3(-1, 0, 0);

            if (!CheckIsValidPosition())
                this.transform.position += new Vector3(1, 0, 0);
            else
                FindObjectOfType<Game>().UpdateGrid(this);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(allowRotation)
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }

                if (!CheckIsValidPosition())
                {
                     if(limitRotation)
                    {
                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, 90);
                        }
                    } else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
                else
                {
                    FindObjectOfType<Game>().UpdateGrid(this);
                }                 
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fall >= speedFall)
        {

            this.transform.position += new Vector3(0, -1, 0);

            if (!CheckIsValidPosition())
            {
                this.transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();

                if(FindObjectOfType<Game>().CheckIsAboveGrid(this))
                {
                    FindObjectOfType<Game>().GameOver();
                }

                FindObjectOfType<Game>().spawnNext();
                Game.currentScore += individualScore;
                enabled = false;
            }
            else
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }

            fall = Time.time;
        }

    }

     bool CheckIsValidPosition()
    {
        foreach(Transform tetris in transform)
        {
            Vector3 pos = FindObjectOfType<Game>().Round(tetris.position);

            if(FindObjectOfType<Game>().CheckIsInsideGrid(pos) == false)
            {
                return false;
            }

            if(FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
