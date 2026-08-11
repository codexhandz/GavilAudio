using GavilAudio.Audio;
using GavilAudio.UI;
using GavilAudio.Commands;
// All the folders that this program.cs has access to. Every .cs needs it unless it doesnt need to access another folder.

Console.SetWindowSize(85, 46);

ConsoleUI ui = new();

AudioManager audio = new();

CommandProcessor processor = new(audio, ui);

//here we create new assets.

string userInput = "";
string message = "";
bool running = true;
// Establishing variable that can be changed. userInput (from user to processor), message (from processor to program), and running (to decide then the program ends)

while (running)
{
    int volume = audio.GetMasterVolume();
    List<AudioDevice> devices = audio.GetOutputDevices();
    ui.ShowConsoleUI(volume, devices);
    ui.ShowLastInteraction(userInput, message);
    // We give ConsoleUI devices info, volume info, last interaction info.
    userInput = ui.GetCommand();
    // We ask ConsoleUI to get new command.

    (bool keepRunning, message) = processor.ProcessCommand(userInput);
    running = keepRunning;
    // Before finishing the loop, check if we repeat it.
}