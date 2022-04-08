using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum deviceTag
{
    swapTag = 0,
    parchmentTag = 1,
    selectionWheelTag = 2,
    hitTag = 3,
    jumpTag = 4

}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Vector3 gravity = new Vector3(0.0f, -9.8f, 0.0f);

    // 0-> swapTag     1-> parchmentTag     2->selectionWheelTag    3-> hitTag    4-> jumpTag
    public List<Image> kbTags;
    public List<Image> xboxTags;
    public List<Image> psTags;

    // Altar present in the level
    private Altar altar;

    // List of enemies in the level
    private List<Enemy> enemies;

    private List<CrackedPlatform> crackedPlatforms;

    private List<MobilePlatform> mobilePlatforms;

    // List of defeated respawnable enemies in the level
    private List<Enemy> _respawnableEnemies;

    // Players
    public PlayerManager player;

    [HideInInspector] public bool usingGamepad;

    // Runes counter script
    [SerializeField] private RunesCounterUI runesCounterUIScript;

    // Number of runes in the level
    private int _numRunes;

    // Number of runes collected in the level
    private int _collectedRunes;

    // Number of deaths in the level
    private int _numDeaths;

    private Color currentBGColor;

    [SerializeField]
    private Light mainLight;

    private AudioSource musicSource;

    private float musicBaseVolume;

    private float volumeLerpStep = 0.0f;

    private Material skybox;
    private Color ambientLight;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _respawnableEnemies = new List<Enemy>();
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _numRunes = GameObject.FindGameObjectsWithTag("Runa").Length;
        _collectedRunes = 0;
        _numDeaths = 0;

        musicSource = GetComponent<AudioSource>();
        musicBaseVolume = musicSource.volume;

        musicSource.volume = 0.0f;

        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        mobilePlatforms = new List<MobilePlatform>(FindObjectsOfType<MobilePlatform>());
        crackedPlatforms = new List<CrackedPlatform>(FindObjectsOfType<CrackedPlatform>());
        altar = FindObjectOfType<Altar>();

        currentBGColor = Camera.main.backgroundColor;

        skybox = RenderSettings.skybox;
        ambientLight = RenderSettings.ambientLight;

    }

    // Update is called once per frame
    void Update()
    {
        if (volumeLerpStep < 1.0f)
        {
            volumeLerpStep += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0.0f, musicBaseVolume, volumeLerpStep);
        }
        else
        {
            musicSource.volume = musicBaseVolume;
        }

        if (Camera.main.transform.position.y < 0.0f)
        {
            RenderSettings.ambientIntensity = 0.0f;
            RenderSettings.reflectionIntensity = 0.0f;
            Camera.main.backgroundColor = new Color();
            RenderSettings.skybox = null;
            RenderSettings.ambientLight = new Color(0, 0, 0);
            mainLight.enabled = false;
        }
        else
        {
            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.reflectionIntensity = 1.0f;
            Camera.main.backgroundColor = currentBGColor;
            RenderSettings.ambientLight = ambientLight;
            RenderSettings.skybox = skybox;
            mainLight.enabled = true;
        }
    }

    public void pickedRune()
    {
        // this method is called when a rune is picked
        // if the player has colleted enough runes, the altar is enabled

        _collectedRunes++;
        if (_collectedRunes >= _numRunes)
        {
            altar.enableAltar();
        }
        runesCounterUIScript.addRune(1);

    }

    public void respawnedPickedRune()
    {
        // this method is called when a rune that was picked, is respawned
        // if a rune is respawned, check if the minimum number of runes required for the altar to be enabled is not met

        _collectedRunes--;
        if (_collectedRunes < _numRunes)
        {
            altar.disableAltar();
        }
        runesCounterUIScript.addRune(-1);
    }

    public void respawnableEnemy(Enemy enemy)
    {
        _respawnableEnemies.Add(enemy);
    }

    public void newDeath()
    {
        // this method is called when the player dies

        _numDeaths++;

        // each defeated respawnable enemie is respawned
        foreach (Enemy enemy in _respawnableEnemies)
            enemy.SpawnEnemy();

        foreach (CrackedPlatform platform in crackedPlatforms)
            platform.ResetPlatform();

    }

    public void pause()
    {
        musicSource.volume = 0.5f * musicBaseVolume;

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
        player.beetleBehaviour.GetComponent<AudioSource>().enabled = false;

        // enemies
        foreach (Enemy enemy in enemies)
        {
            scripts = new List<MonoBehaviour>(enemy.GetComponentsInChildren<MonoBehaviour>());
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
            enemy.GetComponent<Animator>().enabled = false;
        }

        // Mobile platforms
        foreach (MobilePlatform platform in mobilePlatforms)
            platform.enabled = false;

        Camera.main.GetComponent<CinemachineBrain>().enabled = false;

    }

    public void play()
    {
        musicSource.volume = musicBaseVolume;

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
        player.beetleBehaviour.GetComponent<AudioSource>().enabled = true;

        // enemies
        foreach (Enemy enemy in enemies)
        {
            scripts = new List<MonoBehaviour>(enemy.GetComponentsInChildren<MonoBehaviour>());
            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = true;
            }

            enemy.GetComponent<BezierFollow>().enabled = enemy.allowWalking;
            enemy.GetComponent<Animator>().enabled = true;

        }

        // Mobile platforms
        foreach (MobilePlatform platform in mobilePlatforms)
            platform.enabled = true;

        Camera.main.GetComponent<CinemachineBrain>().enabled = true;

    }

    public void OnDeviceChange(PlayerInput context)
    {

        if (context.devices.Count > 0 && kbTags.Count > 0)
        {
            if (context.devices[0].name.StartsWith("Keyboard"))
            {
                // Keyboard gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = true;
                    xboxTags[i].enabled = false;
                    psTags[i].enabled = false;
                }

                if (instance)
                    instance.usingGamepad = false;

            }
            else if (context.devices[0].name.StartsWith("DualShock"))
            {
                // PlayStation gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = false;
                    xboxTags[i].enabled = false;
                    psTags[i].enabled = true;
                }
                if (instance)
                    instance.usingGamepad = true;
            }
            else
            {
                // Xbox gamepad
                for (int i = 0; i < kbTags.Count; i++)
                {
                    kbTags[i].enabled = false;
                    xboxTags[i].enabled = true;
                    psTags[i].enabled = false;
                }

                if (instance)
                    instance.usingGamepad = true;
            }
        }
    }

    public IEnumerator highLightTag(deviceTag deviceTag)
    {
        if (kbTags.Count > 0)
        {
            if (kbTags[0].enabled)
            {
                kbTags[(int)deviceTag].color = new Color(0.676f, 0.676f, 0.676f);
                yield return new WaitForSeconds(0.1f);
                kbTags[(int)deviceTag].color = Color.white;
            }
            else if (xboxTags[0].enabled)
            {
                xboxTags[(int)deviceTag].color = new Color(0.676f, 0.676f, 0.676f);
                yield return new WaitForSeconds(0.1f);
                xboxTags[(int)deviceTag].color = Color.white;
            }
            else if (psTags[0].enabled)
            {
                psTags[(int)deviceTag].color = new Color(0.676f, 0.676f, 0.676f);
                yield return new WaitForSeconds(0.1f);
                psTags[(int)deviceTag].color = Color.white;
            }
        }
    }
}
