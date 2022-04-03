using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KilledEnemiesActivator : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> enemiesToKill;

    [SerializeField]
    private UnityEvent action;

    private bool activated;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Enemy enemy in enemiesToKill)
        {
            if (!enemy._isDefeated)
                return;
        }

        if (!activated)
        {
            action.Invoke();
            activated = true;
        }
    }
}
