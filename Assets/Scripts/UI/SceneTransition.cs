using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    /////////////////////////////////////////  p u b l i c   v a r i a b l e s  ////////////////////////////////////////
    public static SceneTransition sceneTransitioninstance;
    
    ////////////////////////////////////////  p r i v a t e   v a r i a b l e s  ///////////////////////////////////////
    private Animator _transitionAnim;

    
    public IEnumerator LoadScene(int sceneId)
    {
        
        yield return new WaitForSeconds(1.0f);
        _transitionAnim.SetTrigger("start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneId);
    }
    
    private void Awake()
    {
        sceneTransitioninstance = this;
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
