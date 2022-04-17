using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LifesCounterUI : MonoBehaviour
{
    public Image[] lifesUI;
    public Color lifeColor;
    public Color deadColor;

    private int _numLifes;
    
    public void DoDeath()
    {
        // gray coloured hearts
        if (GameManager.instance)
        {
            if (GameManager.instance.numDeaths == GameManager.instance._lifes)
            {
                ResetLifes();
                Cursor.visible = true;
                SceneTransition.instance.LoadScene("DeathScene");
            }
            else
            {
                lifesUI[_numLifes - 1].color = deadColor;
                _numLifes--;
            }
            
        }
    }

    public void ResetLifes()
    {
        // red coloured hearts
        foreach (var image in lifesUI)
        {
            image.color = lifeColor;
        }
        _numLifes = GameManager.instance._lifes;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance)
            _numLifes = GameManager.instance._lifes;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
