using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum DeviceConnected
{
    Keyboard = 0,
    Ps = 1,
    Xbox = 2,
}
public class DeviceControlsManager : MonoBehaviour
{
    
    public static DeviceControlsManager devicesInstance;
    
    
    public DeviceConnected GetDeviceConnected(PlayerInput context)
    {
        if (context.devices[0].name.StartsWith("Keyboard"))
        {
            // Keyboard gamepad
            return DeviceConnected.Keyboard;
    
        }
        else if (context.devices[0].name.StartsWith("DualShock"))
        {
            // PlayStation gamepad
            return DeviceConnected.Ps;
    
        }
        else
        {
            // Xbox gamepad
            return DeviceConnected.Xbox;
        }
    }
    

    public void SetTagsInScene(PlayerInput context, List<Image> kbTags, List<Image> xboxTags, List<Image> psTags)
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
            }
        }
    }
    
    private void Awake()
    {
        
        devicesInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
