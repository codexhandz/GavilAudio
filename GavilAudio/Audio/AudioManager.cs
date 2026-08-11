using NAudio.CoreAudioApi;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GavilAudio.Audio;

public class AudioManager
{
    private readonly MMDeviceEnumerator _deviceEnumerator;
    private readonly MMDevice _defaultDevice;
    private readonly MMDevice _defaultMultimediaDevice;
    private readonly MMDevice _defaultCommunicationsDevice;
    // This builds the object from NAudio to change volume (_deviceEnumerator) and to identify the device(s) (_defaultDevice)
    public AudioManager()
    {
        _deviceEnumerator = new MMDeviceEnumerator();

        _defaultDevice = _deviceEnumerator.GetDefaultAudioEndpoint(
            DataFlow.Render,
            Role.Multimedia);
        // Erase this _defaultDevice later

        _defaultMultimediaDevice = _deviceEnumerator.GetDefaultAudioEndpoint(
            DataFlow.Render, 
            Role.Multimedia);

        _defaultCommunicationsDevice = _deviceEnumerator.GetDefaultAudioEndpoint(
            DataFlow.Render, 
            Role.Communications);
    }
    public int GetMasterVolume()
    {
        float volume = _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
        return (int)(volume * 100);
    }
    //This stores the volume in a float (32-bit number). Because it returns <0-1> we need to convert it to percentage in the return line.
    public List<AudioDevice> GetOutputDevices()
    {
        string defaultMultimediaDevice = _defaultMultimediaDevice.ID;
        string defaultCommunicationsDevice = _defaultCommunicationsDevice.ID;

        List<AudioDevice> outputDeviceList = new List<AudioDevice>();
        foreach (var device in _deviceEnumerator.EnumerateAudioEndPoints(
            DataFlow.Render, 
            DeviceState.Active))
        {
            bool isDefaultForMultimedia = false;
            if (device.ID == defaultMultimediaDevice)
            {
                isDefaultForMultimedia = true;
            }

            bool isDefaultForCommunications = false;
            if (device.ID == defaultCommunicationsDevice)
            {
                isDefaultForCommunications = true;
            }

            outputDeviceList.Add(new AudioDevice() {
                DeviceFriendlyName = device.DeviceFriendlyName,
                Id = device.ID,
                State = device.State,
                IsDefaultForMultimedia = isDefaultForMultimedia,
                IsDefaultForCommunications = isDefaultForCommunications

            });
        }
        return outputDeviceList;
    }
    // We make a List of AudioDevice class to respond to GetOutputDevices. Then we go one object at the time for the object that have Render value and Active and add the following infor to each: DeviceFriendlyName, ID, State. Lastly, we return the list the the requestor.
    public void SetMasterVolume(int volume)
    {
        volume = Math.Clamp(volume, 0, 100);
        float newVolume = (volume / 100f);
        _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;
    }
    // Clamp is to protect the program against invalid values. Then, we convert from percentage to windows value <0-1>. Lastly, we change the volume with MasterVolumeLevelScalar
    public string GetCurrentDeviceName()
    {
        return _defaultDevice.DeviceFriendlyName;
    }

    public bool SetDefaultDevice(string deviceId, ERole role)
    {
        CPolicyConfigClient client = new CPolicyConfigClient();
        IPolicyConfig policyConfig = (IPolicyConfig)client;

        int result = policyConfig.SetDefaultEndpoint(
            deviceId,
            role);

        if (result == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public enum ERole : uint
    {
        eConsole = 0,
        eMultimedia = 1,
        eCommunications = 2,
    }

    //===========================================
    //          CLASS DEFINITION
    //===========================================

    [ComImport]
    [Guid("870af99c-171d-4f9e-af0d-e63df40c2bc9")]
    internal class CPolicyConfigClient
    {
    }

    //===========================================
    //          VTABLE DEFINITIONS
    //===========================================

    [ComImport]
    [Guid("f8679f50-850a-41cf-9c72-430f290290c8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IPolicyConfig
    {
        int GetMixFormat
            (
            string id,
            out IntPtr format
            );
        int GetDeviceFormat();
        int ResetDeviceFormat();
        int SetDeviceFormat();
        int GetProcessingPeriod();
        int SetProcessingPeriod();
        int GetShareMode();
        int SetShareMode();
        int GetPropertyValue();
        int SetPropertyValue();
        int SetDefaultEndpoint
            (
            [In] [MarshalAs(UnmanagedType.LPWStr)] string deviceId,
            [In] [MarshalAs(UnmanagedType.U4)] ERole role
            );
        int SetEndpointVisibility();
    }
}