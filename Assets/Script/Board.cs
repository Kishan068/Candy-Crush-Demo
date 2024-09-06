using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    public int Row, Colum;
    public GameObject[] Allprefeb;
    public GameObject[,] AllCandies;
    public GameObject Key,curMoveCandy;
    public bool IsKey = false;
    GameObject k;
    public Text score;
    int sc;
    MatchFinder Match;
    // Start is called before the first frame update
    void Start()
    {
        sc = 3;
        AllCandies = new GameObject[Colum,Row];
        GanrateBoard();
        score.text = sc.ToString();
        Match = FindObjectOfType<MatchFinder>();
        PlayerPrefs.SetInt("score",sc);
    }
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void reset()
    {
        SceneManager.LoadScene("SampleScene");
    }

    void GanrateBoard()
    {
        for (int a = 0; a < Colum; a++)
        {
            for (int b = 0; b < Row; b++)
            {
                Vector2 pos = new Vector2(a, b);
                int r = Random.Range(0, Allprefeb.Length);
                while (checkIsMatch(a, b, Allprefeb[r]))
                {
                   // print("Got Matched");
                    r = Random.Range(0, Allprefeb.Length);
                }
                GameObject g = Instantiate(Allprefeb[r], pos, Quaternion.identity, transform);
                g.name = "(" + a + "," + b + ")";
                AllCandies[a, b] = g;
                g.GetComponent<Candy>().Row = b;
                g.GetComponent<Candy>().Colum = a;
            }
        }
    }

    void GanrateKey()
    {
        int r = Random.Range(0,5);
        while (AllCandies[r,5] != null)
        {
            r = Random.Range(0,5);
        }
        Vector2 pos = new Vector2(r,5);
        GameObject g = Instantiate(Key, pos, Quaternion.identity);
        g.name = "(" + r + ",5)";
        AllCandies[r, 5] = g;
        g.GetComponent<Candy>().Row = 5;
        g.GetComponent<Candy>().Colum = r;
        IsKey = true;

    }
    public void DestroyCandy()
    {
        print(Match.matchedCandies.Count);
        if(Match.matchedCandies.Count >= 4)
        {
            Match.checkForBomb();
        }
        for (int i = 0; i < Colum; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                if (AllCandies[i, j] != null)
                {
                    if (AllCandies[i, j].GetComponent<Candy>().IsMatch == true)
                    {
                        Destroy(AllCandies[i, j]);
                        AllCandies[i, j] = null;
                    }
                }
            }
        }
        if (IsKey == true)
        {
            if (k.GetComponent<Candy>().Row == 0)
            {
                Destroy(k);
                AllCandies[0, k.GetComponent<Candy>().Colum] = null;
                IsKey = false;
                PlayerPrefs.GetInt("score",sc);
                sc--;
                score.text = sc.ToString();
                PlayerPrefs.SetInt("score",sc);
            }
        }
        StartCoroutine(DownCandy());
    }
    IEnumerator DownCandy()
    {
       
        int nullcnt = 0;
        for (int i = 0; i < Colum; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                if (AllCandies[i, j] == null)
                {
                    nullcnt++;
                }
                else if (nullcnt > 0)
                {
                    AllCandies[i, j].GetComponent<Candy>().Row -= nullcnt;
                    AllCandies[i, j] = null;
                }
            }
            nullcnt = 0;
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DestroyCandies());
    }
    void refillBoard()
    {
        for (int i = 0; i < Colum; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                if (AllCandies[i, j] == null)
                {
                    if (IsKey == false && j == 5)
                    {
                        Vector2 pos = new Vector2(i, 5);
                        k = Instantiate(Key, pos, Quaternion.identity);
                        k.name = "(" + i + "  ,5  )";
                        AllCandies[i, 5] = k;
                        k.GetComponent<Candy>().Row = 5;
                        k.GetComponent<Candy>().Colum = i;
                        IsKey = true;
                    }
                    else
                    {
                        Vector2 pos = new Vector2(i, j);
                        int r = Random.Range(0, Allprefeb.Length);
                        GameObject g = Instantiate(Allprefeb[r], pos, Quaternion.identity,transform);
                        g.name = "(" + i + "," + j + ")";
                        AllCandies[i, j] = g;
                        g.GetComponent<Candy>().Row = j;
                        g.GetComponent<Candy>().Colum = i;
                    }
                }
            }
        }
    }
    IEnumerator DestroyCandies()
    {
        yield return new WaitForSeconds(0.5f);
        refillBoard();
        Match.matchedCandies.Clear();
        for (int i = 0; i < Colum; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                if (AllCandies[i, j] != null)
                {
                    if (AllCandies[i, j].GetComponent<Candy>().IsMatch)
                    {
                        yield return new WaitForSeconds(0.5f);
                        DestroyCandy();
                    }
                }
            }
        }
        
    }
    bool checkIsMatch(int col, int row, GameObject g)
    {
        if (col > 1 && row > 1)
        {
            if (AllCandies[col - 1, row].tag == g.tag && AllCandies[col - 2, row].tag == g.tag)
            {
                return true;
            }
            if (AllCandies[col, row - 1].tag == g.tag && AllCandies[col, row - 2].tag == g.tag)
            {
                return true;
            }
        }
        else if (col > 1)
        {
            if (AllCandies[col - 1, row].tag == g.tag && AllCandies[col - 2, row].tag == g.tag)
            {
                return true;
            }
        }
        else if (row > 1)
        {
            if (AllCandies[col, row - 1].tag == g.tag && AllCandies[col, row - 2].tag == g.tag)
            {
                return true;
            }
        }
        return false;
    }
}
