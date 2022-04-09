using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsScene : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

        
    }

    public void ReturnToMainMenu(InputAction.CallbackContext context)
    {
        if(context.performed)
           SceneManager.LoadScene("MainMenu");
        
    }

    // Update is called once per frame
    void Update()
    {
    }


}
