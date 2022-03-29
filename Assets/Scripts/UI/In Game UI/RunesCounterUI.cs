using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RunesCounterUI : MonoBehaviour
{
    private int nRunes = 0;

    private Text[] runesCounterTexts;

    // Start is called before the first frame update
    void Start()
    {
        runesCounterTexts = GetComponentsInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void addRune(int amount)
    {
        nRunes += amount;
        foreach (Text runesCounterText in runesCounterTexts)
            runesCounterText.text = " " + nRunes;
    }
}
