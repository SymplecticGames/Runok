using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RunesCounterUI : MonoBehaviour
{
    private int nRunes = 0;

    private Text runesCounterText;

    // Start is called before the first frame update
    void Start()
    {
        runesCounterText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(nRunes);
    }

    public void addRune(int amount)
    {
        nRunes += amount;
        runesCounterText.text = " "+nRunes;
    }
}
