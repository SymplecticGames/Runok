using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public static SceneTransition instance;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private Animator _transitionAnim;
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        // No funciona bien, así que por ahora lo hacemos así :_(
        //StartCoroutine(LoadSceneSync(sceneName));
    }

    private IEnumerator LoadSceneSync(string sceneName)
    {
        yield return new WaitForSeconds(1.0f);
        _transitionAnim.SetTrigger("start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }
    
    private void Awake()
    {
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _transitionAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
