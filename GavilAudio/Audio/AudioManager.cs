using NAudio.CoreAudioApi;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GavilAudio.Audio;

public class AudioManager
{
    private readonly MMDeviceEnumerator _deviceEnumerator;
    private readonly MMDevice _defaultDevice;
    // This builds the object from NAudio to change volume (_deviceEnumerator) and to identify the device(s) (_defaultDevice)
    public AudioManager()
    {
        _deviceEnumerator = new MMDeviceEnumerator();

        _defaultDevice = _deviceEnumerator.GetDefaultAudioEndpoint(
            DataFlow.Render,
            Role.Multimedia);
    }
    public int GetMasterVolume()
    {
        float volume = _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
        return (int)(volume * 100);
    }
    //This stores the volume in a float (32-bit number). Because it returns <0-1> we need to convert it to percentage in the return line.
    public List<AudioDevice> GetOutputDevices()
    {
        List<AudioDevice> outputDeviceList = new List<AudioDevice>();
        foreach (var device in _deviceEnumerator.EnumerateAudioEndPoints(
            DataFlow.Render, 
            DeviceState.Active))
        {
            outputDeviceList.Add(new AudioDevice() {
                DeviceFriendlyName = device.DeviceFriendlyName,
                Id = device.ID,
                State = device.State
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
}