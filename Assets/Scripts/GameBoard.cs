using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private PlayerManagement playerHandler;

    public GameObject playerBox;
    public GameObject otherPlayers;
    public GameObject cardIso;
    public GameObject cardFlat;
    public GameObject timerObject;
    public GameObject arrowObject;
    public GameObject topicButtonObject;
    public GameObject blackScreenObject;

    public Positions positions;


    public Timer timer;

    public AudioSource drawCard;
    public AudioSource spinBubble;

    public Rounds round;

    public TextMeshProUGUI userName;
    public TextMeshProUGUI cardLeftText;

    private Card[] cardStack;
    private MockStats mockStats;
    private Avatars avatars;
    private UserNames nameClass;
    private int roundState = 1;
    private bool advanceToState3 = false;
    private bool waitForServerTopicResponse = false;
    private bool serverRespondedToTopic = false;
    private int[] topics;
    private bool timerPhase1Ended = false;

    private bool startGame = true;



    void Start()
    {
        SetUpInitialBoard();
        StartCoroutine(GameStarts());
        //SetupCardStack();
    }

    private void SetUpInitialBoard()
    {
        cardStack = new Card[13];
        mockStats = GameObject.Find("MockStats").GetComponent<MockStats>();
        avatars = GameObject.Find("Canvas").GetComponent<Avatars>();
        nameClass = GameObject.Find("Canvas").GetComponent<UserNames>();
        playerHandler = GameObject.FindWithTag("PlayerTotal").GetComponent<PlayerManagement>();

        for (int i = 1; i <= mockStats.GetTotalNumberOfPlayers(); i++)
        {
            if (mockStats.GetPlayerPosition() == i)
            {
                playerHandler.GetCorrectPlayerObject(i).GetPlayerName();

                GameObject mainBox = Instantiate(playerBox, playerHandler.GetCorrectPlayerObject(i).GetPos(), Quaternion.identity) as GameObject;
                mainBox.name = "Player" + i + "Box";
                mainBox.transform.SetParent(GameObject.Find("Canvas").transform, false);

                /*TextMeshProUGUI playerName = Instantiate(userName, nameClass.getNamePos(i), Quaternion.identity);
                playerName.name = "Player" + i + "NameText";
                playerName.text = playerHandler.GetCorrectPlayerObject(i).GetPlayerName();
                playerName.transform.SetParent(GameObject.Find("Canvas").transform, false);*/
            }
            else
            {
                playerHandler.GetCorrectPlayerObject(i).GetPlayerName();
                GameObject box = Instantiate(otherPlayers, playerHandler.GetCorrectPlayerObject(i).GetPos(), Quaternion.identity) as GameObject;
                box.name = "Player" + i + "Box";
                box.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }

            GameObject avatar = Instantiate(avatars.GetAvatar(playerHandler.GetCorrectPlayerObject(i).GetAvatar()), avatars.GetAvatarPos(i), Quaternion.identity) as GameObject;
            avatar.name = "Player" + i + "Avatar";
            avatar.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }
        GameObject timer = Instantiate(timerObject, new Vector3(383.5f, 259.7f, 0), Quaternion.identity) as GameObject;
        timer.name = "Timer";
        timer.transform.SetParent(GameObject.Find("Canvas").transform, false);
        timer.SetActive(false);
    }


    public void SetupCardStack()
    {
        StartCoroutine(ShuffleCards());
    }


    //this List needs to contain 65 word (5 x 13). Word 0 - 4 is for card 1, word 5 - 9 for card 2 etc
    //This List is a REACT INPUT
    public void SetCardText(string[] cardText)
    {
        int v = 0;

        for (int i = 0; i < cardStack.Length; i++)
        {
            int count = 0;
            string[] sub = new string[5];
            for (int k = v; k < v + 5; k++)
            {
                sub[count] = cardText[k];
                    count++;
            }
            cardStack[i].SetTopics(sub);
            v += 5;
        }
    }


    private void Update()
    {
        if (startGame)
        {
            if (roundState == 1)
            {
                
            }

            //Topic Picking Phase//
            //In here I have to coninuously ask for the topic Choice array which has to be delivered by the Backend
            else if (roundState == 2)
            {
                topics = mockStats.GetTopicInput();

                for (int i = 1; i <= 5; i++)
                {
                    GameObject.Find("TopicVoteNumber" + i.ToString()).GetComponent<TextMeshProUGUI>().text = topics[i-1].ToString();
                }

                //every player has made his choice conditon
                int sum = 0;
                for (int j = 0; j < 5; j++)
                {
                    sum += topics[j];
                }
                //Everyone has set their vote
                //Check for draws
                if(sum >= (mockStats.GetTotalNumberOfPlayers()-1) || timerPhase1Ended)
                {
                    StartCoroutine(RemoveTopicCard()); //remove the topic card and then continue
                    roundState = 3;
                }
            }

            //Topic votes have been send, now we check if there are any duplicate votes//
            else if (roundState == 3 && advanceToState3)
            {
                //I need to check whether there is a category that has the same amount of votes:
                if (topics.Length != topics.Distinct().Count()) //If yes, array contains duplictes
                {
                    if (!waitForServerTopicResponse)//Just for animation purposes
                    {
                        StartCoroutine(DuplicatetTopicVotesAnimationScreen());
                        waitForServerTopicResponse = true;
                    }

                    if (serverRespondedToTopic)//Wait for the Server to pick the topic
                    {

                    }
                }

                timer.DeactivateTimer();
            }
            else if (roundState == 4)
            {

            }
            else
            {

            }
        }
    }


    public void FlipBox(int i)
    {
        spinBubble.Play();
        GameObject.Find("Player" + i.ToString() + "Avatar").SetActive(false);
        GameObject.Find("Player" + i.ToString() + "Box").GetComponent<Animator>().SetBool("flip",true);
    }


    public void DisplayArrow()
    {
        GameObject arrow = Instantiate(arrowObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        arrow.name = "Arrow";
        arrow.transform.SetParent(GameObject.Find("Player" + mockStats.GetActivePlayer() + "Box").transform, false);
        //arrow.transform.localPosition = positions.GetArrowPosition(mockStats.GetActivePlayer() - 1);
        StartCoroutine(DeactivateArrow());
    }


    public void DrawCard(int i)
    {
        StartCoroutine(DrawCardCoroutine(i));
    }

    public void SetTimerPhase1()
    {
        timerPhase1Ended = true;
    }


    IEnumerator GameStarts()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(4f);
        round.StartRound();
    }


    IEnumerator ShuffleCards()
    {
        //Setup Card Stack of 13 Cards in the Middle
        //Here I need to fetch the topics from the Backend (1 Card has 5 Topics)
        float x = -14;
        float y = -124;
        for (int i = 0; i < 13; i++)
        {
            drawCard.Play();
            string cardName = "Card" + i;
            GameObject card = Instantiate(cardIso, new Vector3(x, y, 0f), Quaternion.identity);
            card.transform.SetParent(GameObject.Find("Canvas").transform, false);
            card.name = cardName;
            cardStack[i] = card.GetComponent<Card>();
            //x += 0;
            y += 4;
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.5f);
        //Set up the number of Cards left
        TextMeshProUGUI cardsLeftText = Instantiate(cardLeftText, positions.GetCardLeftTextPosition(), Quaternion.identity);
        cardsLeftText.name = "CardsLeftText";
        cardsLeftText.transform.SetParent(GameObject.Find("Canvas").transform, false);
        SetCardText(mockStats.GetTopicArray());
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator DrawCardCoroutine(int round)
    {
        GameObject.Find("Card" + (12 - round).ToString()).GetComponent<Animator>().SetBool("drawCard", true);
        drawCard.Play();
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("CardsLeftText").GetComponent<TextMeshProUGUI>().text = "< " + (12 - round).ToString() + " Left >";
        GameObject.Find("CardsLeftText").transform.localPosition = positions.DecreaseCardLeftTextPosition();
        Destroy(GameObject.Find("Card" + (12 - round).ToString()));
        GameObject topicCard = Instantiate(cardFlat, new Vector3(0,0,0), Quaternion.identity) as GameObject;
        topicCard.name = "TopicCard";
        topicCard.transform.SetParent(GameObject.Find("Canvas").transform, false);
        yield return new WaitForSeconds(2.5f);
        //TODO here I have to add the correct topics
        float y = 164.6f;
        for (int j = 0; j < 5; j++)
        {
            GameObject topicButton = Instantiate(topicButtonObject, new Vector3(-7.6f, y, 0), Quaternion.identity) as GameObject;
            GameObject.Find("ButtonCounter").name = "ButtonCounter" + (j + 1).ToString();
            GameObject.Find("TopicVoteNumber").name = "TopicVoteNumber" + (j + 1).ToString();
            GameObject.Find("TopicText").GetComponent<TextMeshProUGUI>().text = cardStack[round].GetTopics()[j];
            GameObject.Find("TopicText").name = "TopicText" + (j + 1).ToString();
            topicButton.name = "TopicButton" + (j+1).ToString();
            topicButton.transform.SetParent(GameObject.Find("Interaction").transform, false);

            yield return new WaitForSeconds(0.2f);
            y = y - 79f; 
        }
        timer.StartTimer(30);
        roundState = 2;
    }

    IEnumerator DeactivateArrow()
    {
        yield return new WaitForSeconds(10.8f);
        Destroy(GameObject.Find("Arrow"));
    }


    IEnumerator RemoveTopicCard()
    {
        round.InterruptTimer();
        GameObject.Find("TopicCard").GetComponent<Animator>().SetBool("disappear",true);
        for (int j = 0; j < 5; j++)
        {
            GameObject.Find("TopicButton" + (j + 1).ToString()).GetComponent<Animator>().SetBool("disappear", true);
        }
        yield return new WaitForSeconds(1f);
        Destroy(GameObject.Find("TopicCard"));
        for (int j = 0; j < 5; j++)
        {
            Destroy(GameObject.Find("TopicButton" + (j + 1).ToString()));
        }
        advanceToState3 = true;
    }


    IEnumerator DuplicatetTopicVotesAnimationScreen()
    {
        GameObject blackScreen = Instantiate(blackScreenObject, new Vector3(0,0,-10), Quaternion.identity) as GameObject;
        blackScreen.name = "BlackScreen";
        blackScreen.transform.SetParent(GameObject.Find("Interaction").transform, false);
        yield return new WaitForSeconds(1f);
        round.DisplaySameVoteMessage();
    }
}

