using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum ConnectedDevice
{
    Keyboard = 0,
    Ps = 1,
    Xbox = 2,
}
public class DeviceControlsManager : MonoBehaviour
{
    public static DeviceControlsManager instance;

    [HideInInspector]
    public ConnectedDevice currentDevice;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateDeviceConnection(PlayerInput context)
    {
        if (context.devices[0].name.StartsWith("Keyboard"))
        {
            // Keyboard gamepad
            currentDevice = ConnectedDevice.Keyboard;
        }
        else if (context.devices[0].name.StartsWith("DualShock"))
        {
            // PlayStation gamepad
            currentDevice = ConnectedDevice.Ps;
        }
        else
        {
            // Xbox gamepad
            currentDevice = ConnectedDevice.Xbox;
        }
    }

    public void SetTagsInScene(List<Image> kbTags, List<Image> xboxTags, List<Image> psTags)
    {
        if (currentDevice.Equals(ConnectedDevice.Keyboard))
        {
            // Keyboard Device
            for (int i = 0; i < kbTags.Count; i++)
            {
                kbTags[i].enabled = true;
                xboxTags[i].enabled = false;
                psTags[i].enabled = false;
            }

        }
        else if (currentDevice.Equals(ConnectedDevice.Ps))
        {
            // PlayStation gamepad Device
            for (int i = 0; i < kbTags.Count; i++)
            {
                kbTags[i].enabled = false;
                xboxTags[i].enabled = false;
                psTags[i].enabled = true;
            }
        }
        else
        {
            // Xbox gamepad Device
            for (int i = 0; i < kbTags.Count; i++)
            {
                kbTags[i].enabled = false;
                xboxTags[i].enabled = true;
                psTags[i].enabled = false;
            }
        }
    }
}
