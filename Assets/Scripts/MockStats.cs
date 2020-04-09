using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockStats : MonoBehaviour
{
    private int playerPosition;
    private int playerTotal;
    private int activePlayer;
    private int connectedPlayers;
    private int[] topicChoices; //the topicnumber which this user choose [1,5]

    private string[] names = { "Chris", "Thanh", "Marc", "Ivan", "Simon", "E.T.", "Rambo" };
    private int[] avatar = { 1, 2, 3, 4, 5, 6, 7 };
    string[] input = { null, null, null, null, null, null, null };
    string[] topicArray = { "Fever", "River", "Candy", "Rainbow", "Hammer", "Wrench", "Zebra", "Ivy", "Airplane", "Bridge",
        "Frost", "Lollipop", "Parachute", "Day", "Hammer", "Witch", "Lasso", "Burger", "Lotto Ticket", "Worm",
        "Fire", "Grass", "Parot", "Fear", "Hammer", "Giraffe", "Painting", "Train", "Star", "Cricket",
        "Wave", "Bench", "Comedy", "Monster", "Baby", "Wrench", "Piano", "Laptop", "Singer", "Wasp",
        "Roach", "Dog", "Sand", "Swamp", "Face", "Wrench", "Flute", "PC", "Villa", "Bee",
        "Gun", "Cat", "Night", "Fire", "Iron", "Wrench", "Tears", "Mobilephone", "Tree", "Snake",
        "Stone", "Hero", "Lasergun", "Ladybug", "Spike"};


    // Start is called before the first frame update
    void Start()
    {
        activePlayer = 5;
        playerPosition = 3; //REACTINPUT, this value needs to come from React
        playerTotal = 7; // REACTINPUT, this value needs to come from React
        connectedPlayers = 0; //REACTINPUT, this value needs to come from React
        topicChoices = new int[5]; //In here, each Field represents a Topic (Field 0 = Topic 1; Field 1 = Topic 2). Each Field contains an integer value
                                    //indicating how many votes this Topic has [0,2,0,1,0] means that Topic 2 has 2 votes, Topic 4 has 1 vote, thre rest has 0 votes
    }

    public string GetName(int i)
    {
        return names[i];
    }

    public int GetAvatar(int i)
    {
        return avatar[i];
    }

    public int GetTotalNumberOfPlayers()
    {
        return playerTotal;
    }

    public int GetPlayerPosition()
    {
        return playerPosition;
    }

    public void SetPlayerPosition(int i)
    {
        playerPosition = i;
    }

    public void SetPlayerTotal(int i)
    {
        playerTotal = i;
    }

    public string[] GetTopicArray()
    {
        return topicArray;
    }

    public void SetConnectedPlayers() //this is +1 just for testing, afterwards it needs to pull this number from react
    {
        connectedPlayers += 1;
    }

    public int GetConnectedPlayers()
    {
        return connectedPlayers;
    }

    public int GetActivePlayer()
    {
        return activePlayer;
    }

    //this function will send the topic out to React. React will take the topic number and increment this topic number by +1 in the backend for this game
    //Then, React will ask for the topic info array for all players from the backend and sends this array back to unity mockStats
    //where the topicChoices[] will be updated
    //The Input which I need here from React is int[5], where Field 0 = Amount of Votes for Topic 1; Field 1 = Amount of Votes for Topic 2; etc
    public void SetPlayerTopicInput(int i) //THIS IS A SEND TO REACT METHOD
    {
        topicChoices[i]++; //JUST TESTING
    }

    //this has to call for the topic array from React and update the topicChoices array in here accordingly
    public int[] GetTopicInput()
    {
        //TODO Ask React to get the choices in the backend. React will then itself call the receiveTopicInput() method in here to update topicChoices[]
        return topicChoices; //I will have to remove this later on
    }

    public int[] ReceiveTopicInput(int[] topicVotes)
    {
        topicChoices = topicVotes;
        return topicChoices;
    }


    public void TESTINGsetTopicInputArray()
    {
        topicChoices[0] = 0;
        topicChoices[1] = 2;
        topicChoices[2] = 1;
        topicChoices[3] = 0;
        topicChoices[4] = 2;
    }

}
