using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{
    Board board;
    public List<GameObject> matchedCandies;
    bool IsRowEqual, IsColumEqual;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }
    public void findMatchesOnBoard()
    {
        StartCoroutine(findMatchesCo());
    }
    IEnumerator findMatchesCo()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < board.Colum; i++)
        {
            for (int j = 0; j < board.Row; j++)
            {
                GameObject curCandy = board.AllCandies[i, j];
                if (curCandy != null)
                {
                    if (i > 0 && i < board.Colum - 1)
                    {
                        GameObject leftCandy = board.AllCandies[i - 1, j];
                        GameObject rightCandy = board.AllCandies[i + 1, j];
                        if (leftCandy != null && rightCandy != null)
                        {
                            if (curCandy.tag == leftCandy.tag && curCandy.tag == rightCandy.tag)
                            {
                                print("match");
                                curCandy.GetComponent<Candy>().IsMatch = true;
                                leftCandy.GetComponent<Candy>().IsMatch = true;
                                rightCandy.GetComponent<Candy>().IsMatch = true;
                                if (leftCandy.GetComponent<Candy>().IsColumBomb)
                                {
                                    CollectColumBomb(leftCandy.GetComponent<Candy>().Colum);
                                }
                                if (rightCandy.GetComponent<Candy>().IsColumBomb)
                                {
                                    CollectColumBomb(leftCandy.GetComponent<Candy>().Colum);
                                }
                                if (curCandy.GetComponent<Candy>().IsColumBomb)
                                {
                                    CollectColumBomb(leftCandy.GetComponent<Candy>().Colum);
                                }

                                if (leftCandy.GetComponent<Candy>().IsRowBomb)
                                {
                                    CollectRowBomb(leftCandy.GetComponent<Candy>().Row);
                                }
                                if (rightCandy.GetComponent<Candy>().IsRowBomb)
                                {
                                    CollectRowBomb(leftCandy.GetComponent<Candy>().Row);
                                }
                                if (curCandy.GetComponent<Candy>().IsRowBomb)
                                {
                                    CollectRowBomb(leftCandy.GetComponent<Candy>().Row);
                                }

                                if (!matchedCandies.Contains(leftCandy))
                                {
                                    matchedCandies.Add(leftCandy);
                                }
                                if (!matchedCandies.Contains(rightCandy))
                                {
                                    matchedCandies.Add(rightCandy);
                                }
                                if (!matchedCandies.Contains(curCandy))
                                {
                                    matchedCandies.Add(curCandy);
                                }
                            }
                        }
                    }

                    if (j > 0 && j < board.Row - 1)
                    {
                        GameObject upCandy = board.AllCandies[i, j + 1];
                        GameObject downCandy = board.AllCandies[i, j - 1];
                        if (upCandy != null && downCandy != null)
                        {
                            if (curCandy.tag == upCandy.tag && curCandy.tag == downCandy.tag)
                            {
                                curCandy.GetComponent<Candy>().IsMatch = true;
                                upCandy.GetComponent<Candy>().IsMatch = true;
                                downCandy.GetComponent<Candy>().IsMatch = true;
                                if (curCandy.GetComponent<Candy>().IsColumBomb)
                                {
                                    CollectColumBomb(curCandy.GetComponent<Candy>().Colum);
                                }
                                if (upCandy.GetComponent<Candy>().IsColumBomb)
                                {
                                    CollectColumBomb(upCandy.GetComponent<Candy>().Colum);
                                }
                                if (downCandy.GetComponent<Candy>().IsColumBomb)
                                {
                                    CollectColumBomb(downCandy.GetComponent<Candy>().Colum);
                                }

                                if (curCandy.GetComponent<Candy>().IsRowBomb)
                                {
                                    CollectRowBomb(curCandy.GetComponent<Candy>().Row);
                                }
                                if (upCandy.GetComponent<Candy>().IsRowBomb)
                                {
                                    CollectRowBomb(upCandy.GetComponent<Candy>().Row);
                                }
                                if (downCandy.GetComponent<Candy>().IsRowBomb)
                                {
                                    CollectRowBomb(downCandy.GetComponent<Candy>().Row);
                                }
                                if (!matchedCandies.Contains(upCandy))
                                {
                                    matchedCandies.Add(upCandy);
                                }
                                if (!matchedCandies.Contains(downCandy))
                                {
                                    matchedCandies.Add(downCandy);
                                }
                                if (!matchedCandies.Contains(curCandy))
                                {
                                    matchedCandies.Add(curCandy);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void checkForBomb()
    {
        if (board.curMoveCandy != null)
        {
            if (board.curMoveCandy.GetComponent<Candy>().IsMatch)
            {
                board.curMoveCandy.GetComponent<Candy>().IsMatch = false;
                IsRowEqual = RowBomb();
                IsColumEqual = ColumBomb();
                if (IsRowEqual)
                {
                    board.curMoveCandy.GetComponent<Candy>().createRowBomb();
                }
                if (IsColumEqual)
                {
                    board.curMoveCandy.GetComponent<Candy>().createColumnBomb();
                }
                //board.curMoveCandy.GetComponent<Candy>().createRowBomb();
            }
            else if (board.curMoveCandy.GetComponent<Candy>().OppCandy != null)
            {
                Candy oppCandy = board.curMoveCandy.GetComponent<Candy>().OppCandy.GetComponent<Candy>();
                if (oppCandy.IsMatch)
                {
                    oppCandy.IsMatch = false;
                    IsRowEqual = RowBomb();
                    IsColumEqual = ColumBomb();
                    if (IsRowEqual)
                    {
                        oppCandy.createRowBomb();
                    }
                    if (IsColumEqual)
                    {
                        oppCandy.createColumnBomb();
                    }
                   // oppCandy.createColumnBomb();
                }
            }
        }
    }

    void CollectColumBomb(int Colum)
    {
        for (int i = 0; i < board.Row;i++)
        {
            if (board.AllCandies[Colum,i].tag != "Dhinglo")
            {
                board.AllCandies[Colum, i].GetComponent<Candy>().IsMatch = true;
            }
        }
    }

    void CollectRowBomb(int Row)
    {
        for (int i = 0; i < board.Colum; i++)
        {
            if (board.AllCandies[i,Row].tag != "Dhinglo")
            {
                board.AllCandies[i,Row].GetComponent<Candy>().IsMatch = true;
            }
        }
    }

    bool RowBomb()
    {
        if (matchedCandies.Count > 0)
        {
            int cnt = 0;
            int temp = matchedCandies[0].GetComponent<Candy>().Row;
            for (int i = 0; i < matchedCandies.Count;i++)
            {
                if (matchedCandies[i].GetComponent<Candy>().Row == temp)
                {
                    cnt++;
                }
            }

            if (matchedCandies.Count == cnt)
            {
                return true;
            }
        }
        return false;
    }


    bool ColumBomb()
    {
        if (matchedCandies.Count > 0)
        {
            int cnt = 0;
            int temp = matchedCandies[0].GetComponent<Candy>().Colum;
            for (int i = 0; i < matchedCandies.Count; i++)
            {
                if (matchedCandies[i].GetComponent<Candy>().Colum == temp)
                {
                    cnt++;
                }
            }

            if (matchedCandies.Count == cnt)
            {
                return true;
            }
        }
        return false;
    }
}
