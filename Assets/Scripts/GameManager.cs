using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    /////////////////////////////////////////  s t a t i c   v a r i a b l e s  ////////////////////////////////////////

    public static GameManager instance;
    
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    
    // --------------------------------------------------- runes --------------------------------------------------- //
    // list of runes in the level
    public List<GameObject> runes;

    // minimum number of runes the player has to collect to finish the level
    public int minNumRunesToCollect;
    // ------------------------------------------------------------------------------------------------------------- //
    
    // --------------------------------------------------- altar --------------------------------------------------- //
    // altar present in the level
    public GameObject altar;
    // ------------------------------------------------------------------------------------------------------------- //
    
    // --------------------------------------------------- enemies ------------------------------------------------- //
    // list of enemies in the level
    public List<GameObject> enemies;
    // ------------------------------------------------------------------------------------------------------------- //

    // --------------------------------------------------- player --------------------------------------------------- //
    // players
    public PlayerManager player;
    [HideInInspector] public bool usingGamepad;
    
    // -------------------------------------------------- inGameUI ------------------------------------------------- //
    // runes counter script
    [SerializeField] private RunesCounterUI runesCounterUIScript;
    
    // ------------------------------------------------------------------------------------------------------------- //
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    //
    // --------------------------------------------------- runes --------------------------------------------------- //
    // number of runes in the level
    private int _numRunes;
    
    // number of runes collected in the level
    private int _collectedRunes;
    // ------------------------------------------------------------------------------------------------------------- //
    
    // --------------------------------------------------- enemies ------------------------------------------------- //
    // list of defeated respawnable enemies in the level
    private List<GameObject> _defeatedRespawnableEnemies;
    // ------------------------------------------------------------------------------------------------------------- //
    
    // --------------------------------------------------- player --------------------------------------------------- //
    // number of deaths in the level
    private int _numDeaths;

    // ------------------------------------------------------------------------------------------------------------- //

    private Color currentBGColor;

    [SerializeField]
    private Light mainLight;
    
    
    private void Awake()
    {
        Application.targetFrameRate = 60;

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _defeatedRespawnableEnemies = new List<GameObject>();
        _numRunes = runes.Count;
        _collectedRunes = 0;
        _numDeaths = 0;

        currentBGColor = Camera.main.backgroundColor;

        if (_collectedRunes < minNumRunesToCollect)
        {
            altar.GetComponent<Altar>().disableAltar();
        }
        
    }

    // --------------------------------------------------- runes --------------------------------------------------- //
    public void pickedRune()
    {
        // this method is called when a rune is picked
        // if the player has colleted enough runes, the altar is enabled

        _collectedRunes++;
        if (_collectedRunes >= minNumRunesToCollect)
        {
            altar.GetComponent<Altar>().enableAltar();
        }
        runesCounterUIScript.addRune(1);
    }

    public void respawnedPickedRune()
    {
        // this method is called when a rune that was picked, is respawned
        // if a rune is respawned, check if the minimum number of runes required for the altar to be enabled is not met
        
        _collectedRunes--;
        if (_collectedRunes < minNumRunesToCollect)
        {
            altar.GetComponent<Altar>().disableAltar();
        }
        runesCounterUIScript.addRune(-1);
    }
    // ------------------------------------------------------------------------------------------------------------- //
    
    
    // --------------------------------------------------- enemies ------------------------------------------------- //
    public void defeatedRespawnableEnemy(GameObject enemy)
    {
        _defeatedRespawnableEnemies.Add(enemy);
    }
    // ------------------------------------------------------------------------------------------------------------- //
    
    
    
    // --------------------------------------------------- palyer --------------------------------------------------- //
    public void newDeath()
    {
        // this method is called when the player dies
        
        _numDeaths++;
        
        // each defeated respawnable enemie is respawned
        foreach (var enemy in _defeatedRespawnableEnemies)
        {
            enemy.GetComponent<Enemy>().SpawnEnemy();
        }
        
        // reset list
        _defeatedRespawnableEnemies.Clear();
        
    }

    // ------------------------------------------------------------------------------------------------------------- //
    
        // ----------------------------------------------------- UI ----------------------------------------------------- //

    public void pause()
    {
        
        // golem
        List<MonoBehaviour> scripts = new List<MonoBehaviour>(player.golemBehaviour.GetComponentsInChildren<MonoBehaviour>());
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
        player.golemBehaviour.GetComponent<Animator>().enabled = false;
        
        // beetle
        scripts = new List<MonoBehaviour>(player.beetleBehaviour.GetComponentsInChildren<MonoBehaviour>());
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
        player.beetleBehaviour.GetComponent<Animator>().enabled = false;
        
        // enemies
        foreach (var enemy in enemies)
        {
            scripts = new List<MonoBehaviour>(enemy.GetComponentsInChildren<MonoBehaviour>());
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
            //enemy.GetComponent<Animator>().enabled = false;
        }

        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        
    }

    public void play()
    {

        // golem
        List<MonoBehaviour> scripts = new List<MonoBehaviour>(player.golemBehaviour.GetComponentsInChildren<MonoBehaviour>());
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
        player.golemBehaviour.GetComponent<Animator>().enabled = true;
        
        // beetle
        scripts = new List<MonoBehaviour>(player.beetleBehaviour.GetComponentsInChildren<MonoBehaviour>());
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = true;
        }
        player.beetleBehaviour.GetComponent<Animator>().enabled = true;
        
        // enemies
        foreach (var enemy in enemies)
        {
            scripts = new List<MonoBehaviour>(enemy.GetComponentsInChildren<MonoBehaviour>());
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }
            //enemy.GetComponent<Animator>().enabled = true;
        }
        
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }
    // ------------------------------------------------------------------------------------------------------------- //
    
    // Update is called once per frame
    void Update()
    {
        if (Camera.main.transform.position.y < 0.0f)
        {
            RenderSettings.ambientIntensity = 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
            Camera.main.backgroundColor = new Color();
            mainLight.enabled = false;
        }
        else
        {
            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.reflectionIntensity = 1.0f;
            Camera.main.backgroundColor = currentBGColor;
            mainLight.enabled = true;
        }
    }
}
