using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public int Row, Colum, TargetX, TargetY,PrevRow,prevColum;
    float SwipeAngle;
    Vector2 FirstPos, SecondPos;
    Board board;
    public GameObject OppCandy, FirstCandy,RowBomb,ColumBomb;
    public bool IsMatch,IsRowBomb,IsColumBomb;
    MatchFinder Match;

    // Start is called before the first frame update
    void Start()
    {
        Match = FindObjectOfType<MatchFinder>();
        board = FindObjectOfType<Board>();
    }

    void Update()
    {
        TargetX = Colum;
        TargetY = Row;
        if (Mathf.Abs(transform.position.x - TargetX) > 0.1)
        {
            Vector2 newPos = new Vector2(TargetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, newPos, 0.5f);
            if (this.gameObject != board.AllCandies[Colum, Row])
            {
                board.AllCandies[Colum, Row] = this.gameObject;
            }
        }
        else
        {
            Vector2 newPos = new Vector2(TargetX, transform.position.y);
            transform.position = newPos;
        }

        if (Mathf.Abs(transform.position.y - TargetY) > 0.1)
        {
            Vector2 newPos = new Vector2( transform.position.x,TargetY);
            transform.position = Vector2.Lerp(transform.position, newPos, 0.5f);
            if (this.gameObject != board.AllCandies[Colum, Row])
            {
                board.AllCandies[Colum, Row] = this.gameObject;
            }
        }
        else
        {
            Vector2 newPos = new Vector2( transform.position.x,TargetY);
            transform.position = newPos;
        }
        // isMatched();
        Match.findMatchesOnBoard();
    }
    void isMatched()
    {
        if (Colum > 0 && Row > 0 && Colum < board.Colum - 1 && Row < board.Row - 1)
        {
            GameObject rightCandy = board.AllCandies[Colum + 1, Row];
            GameObject leftCandy = board.AllCandies[Colum - 1, Row];
            GameObject upCandy = board.AllCandies[Colum, Row + 1];
            GameObject downCandy = board.AllCandies[Colum, Row - 1];
            if (rightCandy != null && leftCandy != null)
            {
                if (gameObject.tag == rightCandy.tag && gameObject.tag == leftCandy.tag)
                {
                    IsMatch = true;
                    rightCandy.GetComponent<Candy>().IsMatch = true;
                    leftCandy.GetComponent<Candy>().IsMatch = true;
                }
            }
            if (upCandy != null && downCandy != null)
            {
                if (gameObject.tag == upCandy.tag && gameObject.tag == downCandy.tag)
                {
                    IsMatch = true;
                    upCandy.GetComponent<Candy>().IsMatch = true;
                    downCandy.GetComponent<Candy>().IsMatch = true;
                }
            }
        }
        else if (Row > 0 && Row < board.Row - 1)
        {
            GameObject upCandy = board.AllCandies[Colum, Row + 1];
            GameObject downCandy = board.AllCandies[Colum, Row - 1];
            if (upCandy != null && downCandy != null)
            {
                if (gameObject.tag == upCandy.tag && gameObject.tag == downCandy.tag)
                {
                    IsMatch = true;
                    upCandy.GetComponent<Candy>().IsMatch = true;
                    downCandy.GetComponent<Candy>().IsMatch = true;
                }
            }
        }
        else if (Colum > 0 && Colum < board.Colum - 1)
        {
            GameObject rightCandy = board.AllCandies[Colum + 1, Row];
            GameObject leftCandy = board.AllCandies[Colum - 1, Row];
            if (rightCandy != null && leftCandy != null)
            {
                if (gameObject.tag == rightCandy.tag && gameObject.tag == leftCandy.tag)
                {
                    IsMatch = true;
                    rightCandy.GetComponent<Candy>().IsMatch = true;
                    leftCandy.GetComponent<Candy>().IsMatch = true;
                }
            }
        }
    }


    private void OnMouseDown()
    {
        FirstCandy = this.gameObject;

        FirstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //print("down="+Camera.main.ScreenToWorldPoint( Input.mousePosition));
    }
    private void OnMouseUp()
    {
        SecondPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //print("up=" +);
        calculateAngle();
    }
    void calculateAngle()
    {
        if (Mathf.Abs(SecondPos.x - FirstPos.x) > 0.5f || Mathf.Abs(SecondPos.y - FirstPos.y) > 0.5f)
        {
            board.curMoveCandy = FirstCandy;
            Vector2 offset = new Vector2(SecondPos.x - FirstPos.x, SecondPos.y - FirstPos.y);
            SwipeAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            //print(swipeAngle);
            candyMovement();
        }
    }
    void candyMovement()
    {
        if (SwipeAngle >= -45f && SwipeAngle <= 45f && Colum < board.Colum - 1)
        {
            PrevRow = Row;
            prevColum = Colum;
            OppCandy = board.AllCandies[Colum +1,Row];
            OppCandy.GetComponent<Candy>().Colum -= 1;
            Colum += 1;
        }
        else if (SwipeAngle >= 45f && SwipeAngle <= 135f && Row < board.Row - 1)
        {
            PrevRow = Row;
            prevColum = Colum;
            OppCandy = board.AllCandies[Colum, Row + 1];
            OppCandy.GetComponent<Candy>().Row -= 1;
            Row += 1;
        }
        else if ((SwipeAngle >= 135f || SwipeAngle <= -135f) && Colum > 0)
        {
            PrevRow = Row;
            prevColum = Colum;
            OppCandy = board.AllCandies[Colum - 1, Row];
            OppCandy.GetComponent<Candy>().Colum += 1;
            Colum -= 1;
        }
        else if (SwipeAngle >= -135f && SwipeAngle <= -45f && Row > 0m)
        {
            PrevRow = Row;
            prevColum = Colum;
            OppCandy = board.AllCandies[Colum, Row - 1];
            OppCandy.GetComponent<Candy>().Row += 1;
            Row -= 1;
        }
        StartCoroutine(CheckMoveCandies());
    }
    IEnumerator CheckMoveCandies()
    {
        yield return new WaitForSeconds(0.5f);
        if (OppCandy!= null)
        {
            if (OppCandy.GetComponent<Candy>().IsMatch == false && IsMatch == false)
            {
                OppCandy.GetComponent<Candy>().Row = Row;
                OppCandy.GetComponent<Candy>().Colum = Colum;
                Colum = prevColum;
                Row = PrevRow;
                OppCandy = null;
            }
            else
            {
                board.DestroyCandy();
            }
        }
    }
    public void createRowBomb()
    {
        Instantiate(RowBomb, transform);
        IsRowBomb = true;
    }
    public void createColumnBomb()
    {
        Instantiate(ColumBomb, transform);
        IsColumBomb = true;
    }
}
