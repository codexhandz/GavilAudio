using NAudio.CoreAudioApi;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GavilAudio.Audio;

public class AudioManager : IDisposable
{
    private readonly MMDeviceEnumerator _deviceEnumerator;
    // This builds the object from NAudio to do audio changes and get audio info (_deviceEnumerator) and to identify the device(s) (_defaultDevice)

    public AudioManager()
    {
        _deviceEnumerator = new MMDeviceEnumerator();
    }
    public int GetMasterVolume()
    {
        using (MMDevice defaultDevice =
            _deviceEnumerator.GetDefaultAudioEndpoint(
                DataFlow.Render, 
                Role.Multimedia))
        {
            //float volume =
            //    defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar;

            //return (int)(volume * 100);

            using (var endpointVolume = defaultDevice.AudioEndpointVolume)
            {
                float volume = endpointVolume.MasterVolumeLevelScalar;

                return (int)(volume * 100);
            }
        }
    }

    //This stores the volume in a float (32-bit number). Because it returns <0-1> we need to convert it to percentage in the return line.
    public List<AudioDevice> GetOutputDevices()
    {
        using MMDevice defaultMultimediaDevice =
            _deviceEnumerator.GetDefaultAudioEndpoint(
                DataFlow.Render,
                Role.Multimedia);

        using MMDevice defaultCommunicationsDevice =
            _deviceEnumerator.GetDefaultAudioEndpoint(
                DataFlow.Render,
                Role.Communications);

        string defaultMultimediaDeviceId = defaultMultimediaDevice.ID;
        string defaultCommunicationsDeviceId = defaultCommunicationsDevice.ID;

        List<AudioDevice> outputDeviceList = new List<AudioDevice>();
        foreach (MMDevice device in _deviceEnumerator.EnumerateAudioEndPoints(
            DataFlow.Render,
            DeviceState.Active))
        {
            using (device)
            {
                bool isDefaultForMultimedia = false;
                if (device.ID == defaultMultimediaDeviceId)
                {
                    isDefaultForMultimedia = true;
                }

                bool isDefaultForCommunications = false;
                if (device.ID == defaultCommunicationsDeviceId)
                {
                    isDefaultForCommunications = true;
                }

                outputDeviceList.Add(new AudioDevice()
                {
                    DeviceFriendlyName = device.DeviceFriendlyName,
                    Id = device.ID,
                    State = device.State,
                    IsDefaultForMultimedia = isDefaultForMultimedia,
                    IsDefaultForCommunications = isDefaultForCommunications

                });
            }
        }

        return outputDeviceList;
    }

    // We make a List of AudioDevice class to respond to GetOutputDevices. Then we go one object at the time for the object that have Render value and Active and add the following infor to each: DeviceFriendlyName, ID, State. Lastly, we return the list the the requestor.
    public void SetMasterVolume(int volume)
    {
        volume = Math.Clamp(volume, 0, 100);
        float newVolume = volume / 100f;

        using (MMDevice defaultDevice =
            _deviceEnumerator.GetDefaultAudioEndpoint(
                DataFlow.Render,
                Role.Multimedia))
        {
            using (AudioEndpointVolume endpointVolume = defaultDevice.AudioEndpointVolume)
            {
                endpointVolume.MasterVolumeLevelScalar = newVolume;
            }
        }
    }

    public bool SetDefaultDevice(string deviceId, ERole role)
    {
        CPolicyConfigClient client = new CPolicyConfigClient();

        try
        {
            IPolicyConfig policyConfig = (IPolicyConfig)client;

            int result = policyConfig.SetDefaultEndpoint(
                deviceId,
                role);

            return result == 0;
            // This return automatically return boolean value
        }
        finally
        {
            Marshal.ReleaseComObject(client);
        }
    }

    public enum ERole : uint
    {
        eConsole = 0,
        eMultimedia = 1,
        eCommunications = 2,
    }

    public void Dispose()
    {
        _deviceEnumerator.Dispose();
    }

    // COM IMPORTS,  IPOLICYCONFIG, AND VTABLE
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