using GavilAudio.Audio;
using GavilAudio.UI;
using System.Collections.Generic;

namespace GavilAudio.Commands;

public class CommandProcessor
{
    private readonly AudioManager _audio; //we create the local _audio "shelve" to store "audio" in
    private readonly ConsoleUI _ui; //we vreate a place to put ui locally

    public CommandProcessor(AudioManager audio, ConsoleUI ui) //we create the executor with the "audio" object
    {
        _audio = audio; //audio is now stored in _audio
        _ui = ui;
    }
    public (bool keepRunning, string message) ProcessCommand(string userInput) //accepts userInput (string format) under ProcessCommand message.
                                                                               //Promises two returns, a boolean (keepRunning) and a string (message)
    {
        if (string.IsNullOrWhiteSpace(userInput)) //check to see if userInput string is null or spaces.
        {
            // later it needs to return _ui.ShowError001();
            return (true, _ui.GetErrorNoCommand());
        }
        else //the group below will then start identifying which command the user sent.
        {
            string[] parts = userInput.Split(' ');//splits userInput and puts it in "parts"; parts[0], parts[1], etc.
                if (parts[0] == "exit")
                {
                    return (false, string.Empty); //will send a request to program.cs to shut down program
                }
                else if (parts[0] == "help")
                {
                    return (true, _ui.ShowHelp());
                }
                else if (parts[0] == "volume")
            {
                if (parts.Length > 1) //if there is more than one part, then proceed to the group below where it will try to do a TryParse parts[1].
                    {
                        bool success = int.TryParse(parts[1], out var setVolume); 
                        //Explanation of line above:
                        //1. creates a boolean variable "success" for validation.
                        //2. will try to parse part[1] and send the value to setVolume as integer.
                        //3. if successfully parse, then success = true. If not, then success = false.
                        if (success)
                        {
                            _audio.SetMasterVolume(setVolume); //sends the validated number to SetMasterVolume to change. _audio will clamp 0-100.
                        return (true, $"Changed Master Volume to " + parts[1] + "%");
                        }
                        else
                        {
                        return (true, _ui.GetErrorInvalidVolumeNumber());
                    }
                    }
                    else
                    {
                    return (true, _ui.GetErrorInvalidVolumeNumber());
                    }
                }
            else if (parts[0] == "status")
                {
                return (true, "Status Refreshed."); //doesnt really do anything other than refresh the UI and the user will see the status below header
                }
                else
                {
                return (true, _ui.GetErrorInvalidCommand());
                }
        }
    }
}