using GavilAudio.Audio;
using GavilAudio.UI;
using GavilAudio.Commands; //all the folders that this program.cs has access to.

ConsoleUI ui = new();
AudioManager audio = new();
CommandProcessor processor = new(audio, ui); //here we create new assets.

string userInput = ""; //these two lines will establish userInput and commandRessult variables with placeholder
string message = "";
bool running = true; //boolean value with the name "running" that is true. boolean can only be true or false.
                     //I established this to queue the program when to run or stop.

while (running) //first loop, continue the loop inside while the variable "running" is "true".
{
    int volume = audio.GetMasterVolume(); //these two lines will get info. I relocated them here at the top of the loop so it refreshed every time.
    string currentDevice = audio.GetCurrentDeviceName();

    ui.ShowHeader(); //next three lines will display initial info (header, status, and menu,
    ui.ShowCurrentStatus(currentDevice, volume);
    ui.ShowMenu();
    ui.ShowLastInteraction(userInput, message);

    userInput = ui.GetCommand(); //asks user to input and over-writes userInput

    (bool keepRunning, message) = processor.ProcessCommand(userInput); //sends the input to process command for processing and overwrites commandResult
    running = keepRunning;
}