using GavilAudio.Audio;

Console.WriteLine("=================================");
Console.WriteLine("       GavilAudio v1.0");
Console.WriteLine("=================================");

AudioController audio = new AudioController();

audio.SetMasterVolume(25);

int volume = audio.GetMasterVolume();

Console.WriteLine($"Current Master Volume: {volume}%");