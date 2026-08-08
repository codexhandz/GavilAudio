using NAudio.CoreAudioApi;

namespace GavilAudio.Audio
{
    public class AudioDevice
    {
        public string DeviceFriendlyName { get; set; }
        public string Id { get; set; }
        public DeviceState State { get; set; }
        public bool IsDefaultForMultimedia { get; set; }
        public bool IsDefaultForCommunications { get; set; }
    }
}