using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayGifs : MonoBehaviour
{

    public Sprite[] animatedImages;
    private Image animatedImageObj;
    
    // Start is called before the first frame update
    void Start()
    {
        animatedImageObj = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animatedImages.Length > 0)
        {
            animatedImageObj.sprite = animatedImages[(int) (Time.time * 10) % animatedImages.Length];
        }
    }
}
