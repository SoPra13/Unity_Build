using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private bool interrupt = false;

    public GameObject[] numbers = new GameObject[30];
    void Start()
    {
        
    }


    public void StartTimer(int timeInSeconds)
    {
        StartCoroutine(TimerRuns(timeInSeconds));
    }

    public void DeactivateTimer()
    {
        interrupt = true;
    }


    IEnumerator TimerRuns(int timeInSeconds)
    {
        for(int i = timeInSeconds-1; i >= 0; i--)
        {
            GameObject timeNumber = Instantiate(numbers[i], new Vector3(382.7f, 278.7f, 0), Quaternion.identity) as GameObject;
            timeNumber.name = "TimerNumber";
            timeNumber.transform.SetParent(GameObject.Find("Canvas").transform, false);
            if (interrupt)
            {
                Destroy(timeNumber);
                i = 0;
            }
            yield return new WaitForSeconds(1f);
            Destroy(timeNumber);
        }
        interrupt = false;
    }
}
