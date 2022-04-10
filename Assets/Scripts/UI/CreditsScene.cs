using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsScene : MonoBehaviour
{
    public PlayerInput thisContext;
    
    public List<Image> kbTags;
    public List<Image> xboxTags;
    public List<Image> psTags;
    
    public void OnDeviceChange(PlayerInput context)
    {
        if(DeviceControlsManager.devicesInstance)
            DeviceControlsManager.devicesInstance.SetTagsInScene(context, kbTags, xboxTags, psTags);    
    }
    
    // Start is called before the first frame update
    void Start()
    {
        DeviceControlsManager.devicesInstance.SetTagsInScene(thisContext, kbTags, xboxTags, psTags);
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
