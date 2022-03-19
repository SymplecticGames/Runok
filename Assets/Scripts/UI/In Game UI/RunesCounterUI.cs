using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RunesCounterUI : MonoBehaviour
{
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    private int nRunes = 0;

    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    public Text runesCounterText;


    public void addRune(int amount)
    {
        nRunes += amount;
        runesCounterText.text = " "+nRunes;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
