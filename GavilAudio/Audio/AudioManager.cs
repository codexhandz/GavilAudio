using NAudio.CoreAudioApi;

namespace GavilAudio.Audio;

public class AudioManager
{
    private readonly MMDeviceEnumerator _deviceEnumerator;
    private readonly MMDevice _defaultDevice;

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
    public void ListOutputDevices()
    {
        foreach (var device in _deviceEnumerator.EnumerateAudioEndPoints(
            DataFlow.Render,
            DeviceState.Active))
        {
            Console.WriteLine(device.DeviceFriendlyName);
        }
    }
    public void SetMasterVolume(int volume)
    {
        float newVolume = (volume / 100f);

        volume = Math.Clamp(volume, 0, 100);

        _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = newVolume;
    }
    public string GetCurrentDeviceName()
    {
        return _defaultDevice.DeviceFriendlyName;
    }
}