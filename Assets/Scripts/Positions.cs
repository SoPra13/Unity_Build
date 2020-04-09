﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour
{
    public MockStats mockStats;

    private float cardLeftTextX=-16f;
    private float cardLeftTextY=-86f;

    Vector3[] mainPosArray = { new Vector3(437.0f, -198.0f, 0.0f), new Vector3(141.0f, -271.0f, 0.0f), new Vector3(-168.0f, -267.0f, 0.0f),
    new Vector3(-469.0f, -159.0f, 0.0f), new Vector3(-288.0f, 11.0f, 0.0f), new Vector3(-5.0f, 48.0f, 0.0f), new Vector3(298.0f, 10.0f, 0.0f)};

    Vector3[] posOffArray = { new Vector3(437.0f, -198.0f, 0.0f), new Vector3(141.0f, -271.0f, 0.0f), new Vector3(-168.0f, -267.0f, 0.0f),
    new Vector3(-469.0f, -159.0f, 0.0f), new Vector3(-288.0f, 11.0f, 0.0f), new Vector3(-5.0f, 48.0f, 0.0f), new Vector3(298.0f, 10.0f, 0.0f)};

    Vector3[] dustPositions = { new Vector3(437.0f, -198.0f, 0.0f), new Vector3(141.0f, -271.0f, 0.0f), new Vector3(-168.0f, -267.0f, 0.0f),
    new Vector3(-469.0f, -159.0f, 0.0f), new Vector3(-288.0f, 11.0f, 0.0f), new Vector3(-5.0f, 48.0f, 0.0f), new Vector3(298.0f, 10.0f, 0.0f)};

    Vector3[] arrowPositions = { new Vector3(437.0f, -198.0f, 0.0f), new Vector3(141.0f, -271.0f, 0.0f), new Vector3(-168.0f, -267.0f, 0.0f),
    new Vector3(-469.0f, -159.0f, 0.0f), new Vector3(-288.0f, 11.0f, 0.0f), new Vector3(-5.0f, 48.0f, 0.0f), new Vector3(298.0f, 10.0f, 0.0f)};

    Vector3 cardTextPosition;


    // Start is called before the first frame update
    void Start()
    {
        cardTextPosition = new Vector3(cardLeftTextX, cardLeftTextY, 0);
    }

    public Vector3 GetMainPosition(int i)
    {
        return mainPosArray[i];
    }

    public Vector3 GetOffPosition(int i)
    {
        return posOffArray[i];
    }

    public Vector3 GetDustPosition(int i)
    {
        return dustPositions[i];
    }

    public Vector3 GetArrowPosition(int i)
    {
        return arrowPositions[i];
    }

    public Vector3 GetCardLeftTextPosition()
    {
        return cardTextPosition;
    }

    public Vector3 DecreaseCardLeftTextPosition()
    {
        cardLeftTextX += 3;
        cardLeftTextY -= 3;
        cardTextPosition = new Vector3(cardLeftTextX, cardLeftTextY, 0);
        return cardTextPosition;
    }

}
