using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [HideInInspector]
    public bool[] completedLevel = { false, false, false, false };

    [HideInInspector]
    public int currentCompletedLevels;
    public static ProgressManager instance;

    [HideInInspector]
    public int currentLevel;

    [HideInInspector] public bool firstTime = true;
    
    private void Awake()
    {
        if (!instance)
        {
            currentCompletedLevels = 0;
            currentLevel = 0;

            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateCompletedLevels()
    {
        if (currentCompletedLevels < 2)
            currentCompletedLevels++;
    }

}
