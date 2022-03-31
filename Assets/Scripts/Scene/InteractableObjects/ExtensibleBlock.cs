using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtensibleBlock : MonoBehaviour
{
    [SerializeField]
    private GameObject[] boxes;
    private Vector3[] initialBoxPos;

    [SerializeField]
    private float growingFactor;

    private bool startGrowing;
    private float growingStep;
    private int currentBox;

    // Start is called before the first frame update
    void Start()
    {
        initialBoxPos = new Vector3[boxes.Length];

        for (int i = 0; i < initialBoxPos.Length; i++)
            initialBoxPos[i] = boxes[i].transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (startGrowing)
        {
            growingStep += Time.deltaTime * growingFactor;
            boxes[currentBox].transform.localPosition = Vector3.Lerp(Vector3.zero, initialBoxPos[currentBox], growingStep);

            if(growingStep >= 1.0f){
                startGrowing = false;
                growingStep = 0.0f;
            }   
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Hitter"))
            return;

        if (GameManager.instance.player.currentCharacter.TryGetComponent(out GolemBehaviour golem))
        {
            if (golem.currentMaterial == GolemMaterial.Wooden && !startGrowing)
            {
                // Scaling
                currentBox = (currentBox + 1) % boxes.Length;

                if(currentBox <= 0)
                {
                    for (int i = 1; i < initialBoxPos.Length; i++)
                        boxes[i].SetActive(false);
                }
                else
                {
                    startGrowing = true;
                    boxes[currentBox].SetActive(true);
                }
            }
        }
        else
        {
            // Play sound of hitting a wall
        }
    }
}
