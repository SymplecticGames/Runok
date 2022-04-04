using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [HideInInspector]
    public bool[] completedLevel = { false, false, false, false };

    [HideInInspector]
    public int currentCompletedLevels = -1;
    public static ProgressManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void UpdateCompletedLevels()
    {
        if (currentCompletedLevels < 1)
            currentCompletedLevels++;
    }

}
