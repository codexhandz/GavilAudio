using GavilAudio.Audio;
using GavilAudio.UI;
using GavilAudio.Commands;
using System.Collections.Generic;

ConsoleUI ui = new();
using AudioManager audio = new AudioManager();
CommandProcessor processor = new(audio, ui);

string userInput = "";
string message = "";
string inputBuffer = ""; // <--- NEW: Remembers what the user is currently typing across refreshes
bool running = true;

while (running)
{
    int volume = audio.GetMasterVolume();
    List<AudioDevice> devices = audio.GetOutputDevices();
    var sessions = audio.GetActiveAudioSessions();

    ui.ShowConsoleUI(volume, devices, sessions);
    ui.ShowLastInteraction(userInput, message);

    // Wait for up to 5 seconds. If the user types, inputBuffer captures it.
    string? newCommand = ui.GetCommand(ref inputBuffer, 5000);

    // If newCommand is NOT null, it means the user pressed Enter. Process the command.
    // If it IS null, it means 5 seconds passed. The loop skips this block and redraws the UI from the top!
    if (newCommand != null)
    {
        userInput = newCommand;
        (bool keepRunning, string returnMessage) = processor.ProcessCommand(userInput);
        running = keepRunning;
        message = returnMessage;
    }
}