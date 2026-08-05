using GavilAudio.Audio;
using GavilAudio.UI;

ConsoleUI ui = new();
AudioManager audio = new();

int volume = audio.GetMasterVolume();
string currentDevice = audio.GetCurrentDeviceName();

ui.ShowHeader();
ui.ShowCurrentStatus(currentDevice, volume);

string userInput = ui.GetVolumeInput();

Console.WriteLine();
Console.WriteLine($"You entered: {userInput}");