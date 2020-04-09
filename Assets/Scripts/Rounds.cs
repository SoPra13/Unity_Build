using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rounds : MonoBehaviour
{
    public GameObject infoBar;
    public GameObject infoBarText;
    public GameObject dust;
    public GameObject cardFlat;
    public GameBoard gameBoard;


    public Positions positions;

    public MockStats mockStats;

    public AudioSource notification;
    public AudioSource lockSound;
    public AudioSource clockTick;
    public AudioSource timeOver;

    private int round = 0; //keeps track of the round number, starts with 1, ends after 13
    private Animator infoBarAnimator;
    private Animator dustAnimator;
    private Animator infoTextAnimator;
    private TextMeshProUGUI tmproInfoText;
    private int timer;



    void Start()
    {
        mockStats = GameObject.Find("MockStats").GetComponent<MockStats>();

        GameObject infoBoard = Instantiate(infoBar, new Vector3(-12f, 260f, 0), Quaternion.identity) as GameObject;
        infoBoard.name = "Infoboard";
        infoBoard.transform.SetParent(GameObject.Find("Canvas").transform, false);

        GameObject infoText = Instantiate(infoBarText, new Vector3(-75f, 285f, 0), Quaternion.identity) as GameObject;
        infoText.name = "InfoText";
        infoText.transform.SetParent(GameObject.Find("Canvas").transform, false);

        infoBarAnimator = infoBoard.GetComponent<Animator>();
        infoTextAnimator = infoText.GetComponent<Animator>();
        tmproInfoText = infoText.GetComponent<TextMeshProUGUI>();
    }

    public void StartRound()
    {
        StartPhase1();
    }


    private void StartPhase1()
    {
        notification.Play();
        tmproInfoText.text =  mockStats.GetName(mockStats.GetActivePlayer()) + " has been chosen to be the Active Player this Round! He will draw <color=#001AF6>13</color> random topic cards...";
        gameBoard.DisplayArrow();
        infoBarAnimator.SetBool("displayInfoBar", true);
        infoTextAnimator.SetBool("wake", true);
        StartCoroutine(Round1Shuffle());
    }


    private void StartPhase2()
    {

    }


    private void StartPhase3()
    {

    }


    private void StartPhase4()
    {

    }

    public void InterruptTimer()
    {
        timer = -1;
    }

    public void DisplaySameVoteMessage()
    {
        StartCoroutine(Round1VoteConflict());
    }


    IEnumerator Round1Shuffle()
    {
        yield return new WaitForSeconds(4f);
        gameBoard.SetupCardStack();
        yield return new WaitForSeconds(4f);
        infoTextAnimator.SetBool("wake", false);
        yield return new WaitForSeconds(0.75f);
        notification.Play();
        tmproInfoText.text = "The Cards have been dealt! Round 1 will start with " + mockStats.GetName(mockStats.GetActivePlayer()) + " as <color=#001AF6>active player</color>!";
        infoTextAnimator.SetBool("wake", true);
        yield return new WaitForSeconds(2f);

        //Scenario for non-active Player and Active Player
        if(mockStats.GetActivePlayer() == mockStats.GetPlayerPosition())
        {

        }
        else
        {
            gameBoard.FlipBox(mockStats.GetActivePlayer());
            yield return new WaitForSeconds(1.5f);
            GameObject dustAnimation = Instantiate(dust, new Vector3(-12f, 260f, 0), Quaternion.identity) as GameObject;
            dustAnimation.name = "Dust";
            dustAnimation.transform.SetParent(GameObject.Find("Canvas").transform, false);
            yield return new WaitForSeconds(0.1f);
            GameObject.Find("Dust").transform.localPosition = positions.GetDustPosition(mockStats.GetActivePlayer() - 1);
            dustAnimator = dustAnimation.GetComponent<Animator>();
            yield return new WaitForSeconds(0.25f);
            dustAnimator.SetBool("animateDust", true);
            lockSound.Play();
            yield return new WaitForSeconds(1.2f);
            Destroy(GameObject.Find("Dust"));
        }
        StartCoroutine(Round1PickTopic());
    }


    IEnumerator Round1PickTopic()
    {
        yield return new WaitForSeconds(0.1f);
        //Scenario for non-active Player and Active Player
        if (mockStats.GetActivePlayer() == mockStats.GetPlayerPosition())
        {

        }
        else
        {
            gameBoard.DrawCard(round);
            yield return new WaitForSeconds(1f);
            infoTextAnimator.SetBool("wake", false);
            yield return new WaitForSeconds(1.3f);
            notification.Play();
            tmproInfoText.text = "You have <color=#001AF6>30</color> Seconds to pick a Topic for this Round!";
            infoTextAnimator.SetBool("wake", true);
            yield return new WaitForSeconds(1.5f);
            for (timer = 30; timer >= 0; timer--)
            {
                if (timer != 0) { yield return new WaitForSeconds(0.25f); }
                if (timer <= 3) { clockTick.Play();}
                if (timer != 0) { yield return new WaitForSeconds(0.25f); }
                if (timer <= 10 || timer <= 3) { clockTick.Play(); }
                if (timer != 0) { yield return new WaitForSeconds(0.25f); }
                if (timer <= 3) { clockTick.Play(); }
                if (timer != 0) { yield return new WaitForSeconds(0.25f); }
                if (timer != 0)
                {
                    clockTick.Play();
                }
                else
                {
                    timeOver.Play();
                }
            }
            StartPhase2();
        }
    }

    IEnumerator Round1VoteConflict()
    {
        infoTextAnimator.SetBool("wake", false);
        yield return new WaitForSeconds(0.75f);
        notification.Play();
        tmproInfoText.text = "The Cards have been dealt! Round 1 will start with " + mockStats.GetName(mockStats.GetActivePlayer()) + " as <color=#001AF6>active player</color>!";
        infoTextAnimator.SetBool("wake", true);
        yield return new WaitForSeconds(2f);
    }
}
