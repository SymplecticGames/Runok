using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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

    // lifes
    [HideInInspector]
    public int _lifes;
    public LifesCounterUI lifesCounterUI;

    // Camera
    public float _basecmYSpeed;
    public float _basecmXSpeed;

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

    private List<BalancePlatform> balancePlatforms;

    private List<LightBullet> bullets;

    // List of defeated respawnable enemies in the level
    private List<Enemy> _respawnableEnemies;

    // GolemBoss in the level
    private GolemBoss golemBoss;

    // BeetleBoss in the level
    private BeetleBoss beetleBoss;

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
    [HideInInspector]
    public int numDeaths;

    private Color currentBGColor;

    [SerializeField]
    private Light mainLight;

    public AudioSource musicSource;

    [HideInInspector] public float musicBaseVolume;
    [HideInInspector] public float targetMusicBaseVolume;

    private float volumeLerpStep;

    private Material skybox;
    private Color ambientLight;

    private bool _isPaused;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _respawnableEnemies = new List<Enemy>();
        instance = this;
        _lifes = 5;

    }

    // Start is called before the first frame update
    void Start()
    {
        if (ProgressManager.instance)
            ProgressManager.instance.fightingBoss = SceneManager.GetActiveScene().name.Contains("Boss");
        
        _numRunes = GameObject.FindGameObjectsWithTag("Runa").Length;
        _collectedRunes = 0;
        numDeaths = 0;

        volumeLerpStep = 0.0f;
        musicBaseVolume = musicSource.volume;
        targetMusicBaseVolume = musicSource.volume;

        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        mobilePlatforms = new List<MobilePlatform>(FindObjectsOfType<MobilePlatform>());
        crackedPlatforms = new List<CrackedPlatform>(FindObjectsOfType<CrackedPlatform>());
        balancePlatforms = new List<BalancePlatform>(FindObjectsOfType<BalancePlatform>());
        bullets = new List<LightBullet>(FindObjectsOfType<LightBullet>(true));
        altar = FindObjectOfType<Altar>();
        golemBoss = FindObjectOfType<GolemBoss>();
        beetleBoss = FindObjectOfType<BeetleBoss>();

        currentBGColor = Camera.main.backgroundColor;

        skybox = RenderSettings.skybox;
        ambientLight = RenderSettings.ambientLight;

        ChangeSensitivity(1.0f);
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        if (instance)
            instance.usingGamepad = !DeviceControlsManager.instance.currentDevice.Equals(ConnectedDevice.Keyboard);

        if (volumeLerpStep < 1.0f)
        {
            volumeLerpStep += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(musicSource.volume, targetMusicBaseVolume, volumeLerpStep);

        }
        else if (!_isPaused)
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

        numDeaths++;
        lifesCounterUI.DoDeath();

        // each defeated respawnable enemie is respawned
        foreach (Enemy enemy in _respawnableEnemies)
            enemy.SpawnEnemy();

        foreach (CrackedPlatform platform in crackedPlatforms)
            platform.ResetPlatform();

    }

    public void pause()
    {
        Cursor.visible = true;
    
        _isPaused = true;
        volumeLerpStep = 0.0f;
        targetMusicBaseVolume = 0.05f * musicBaseVolume;

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

        // Cracked platforms
        foreach (CrackedPlatform platform in crackedPlatforms)
            platform.enabled = false;

        // Balance platforms
        foreach (BalancePlatform platform in balancePlatforms)
            platform.enabled = false;

        // Bullets
        foreach (LightBullet bullet in bullets)
            bullet.enabled = false;

        Camera.main.GetComponent<CinemachineBrain>().enabled = false;

        if (golemBoss)
        {
            List<MonoBehaviour> bossScripts = new List<MonoBehaviour>(golemBoss.GetComponentsInChildren<MonoBehaviour>());
            foreach (MonoBehaviour script in bossScripts)
                script.enabled = false;

            List<Animator> anims = new List<Animator>(golemBoss.GetComponentsInChildren<Animator>());
            foreach (Animator anim in anims)
                anim.enabled = false;
        }

        if (beetleBoss)
        {
            List<MonoBehaviour> bossScripts = new List<MonoBehaviour>(beetleBoss.transform.parent.GetComponentsInChildren<MonoBehaviour>());
            foreach (MonoBehaviour script in bossScripts)
                script.enabled = false;

            beetleBoss.GetComponentInChildren<Animator>().enabled = false;

            if (beetleBoss.isShooting)
                beetleBoss.StopAllCoroutines();
        }
    }

    public void play()
    {
        Cursor.visible = false;
        
        _isPaused = false;
        volumeLerpStep = 0.0f;
        targetMusicBaseVolume = musicBaseVolume;

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

        // Cracked platforms
        foreach (CrackedPlatform platform in crackedPlatforms)
            platform.enabled = true;

        // Balance platforms
        foreach (BalancePlatform platform in balancePlatforms)
            platform.enabled = true;

        // Bullets
        foreach (LightBullet bullet in bullets)
            bullet.enabled = true;

        Camera.main.GetComponent<CinemachineBrain>().enabled = true;

        if (golemBoss)
        {
            List<MonoBehaviour> bossScripts = new List<MonoBehaviour>(golemBoss.GetComponentsInChildren<MonoBehaviour>());
            foreach (MonoBehaviour script in bossScripts)
                script.enabled = true;
            List<Animator> anims = new List<Animator>(golemBoss.GetComponentsInChildren<Animator>());
            foreach (Animator anim in anims)
                anim.enabled = true;
        }

        if (beetleBoss)
        {
            List<MonoBehaviour> bossScripts = new List<MonoBehaviour>(beetleBoss.transform.parent.GetComponentsInChildren<MonoBehaviour>());
            foreach (MonoBehaviour script in bossScripts)
                script.enabled = true;

            beetleBoss.GetComponentInChildren<Animator>().enabled = true;

            if(beetleBoss.isShooting)
                beetleBoss.StartCoroutine(beetleBoss.BulletSpawn());
        }
    }

    public void OnDeviceChange(PlayerInput context)
    {
        if (!DeviceControlsManager.instance)
            return;

        DeviceControlsManager.instance.UpdateDeviceConnection(context);
        DeviceControlsManager.instance.SetTagsInScene(kbTags, xboxTags, psTags);
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

    public void ChangeSensitivity(float factor)
    {
        if (!player.freelookCam)
            return;

        player.freelookCam.m_XAxis.m_MaxSpeed = _basecmXSpeed * factor;
        player.freelookCam.m_YAxis.m_MaxSpeed = _basecmYSpeed * factor;
    }
}
