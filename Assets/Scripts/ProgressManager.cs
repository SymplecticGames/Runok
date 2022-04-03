using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [HideInInspector]
    public bool[] completedLevel = {false, false, false, false};
    [HideInInspector]
    public int actualCompletedLevels;
    public static ProgressManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(this);
        actualCompletedLevels = 1;
    }

    public void UpdateCompletedLevels()
    {
        if (actualCompletedLevels < 1)
            actualCompletedLevels++;
    }
    
}
