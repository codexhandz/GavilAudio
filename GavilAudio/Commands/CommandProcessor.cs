using GavilAudio.Audio;
using GavilAudio.UI;
using System;
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
            return (true, _ui.GetErrorNoCommand());
        }

        //the group below will then start identifying which command the user sent.
        string[] parts = userInput.Split(' ', StringSplitOptions.RemoveEmptyEntries); //splits userInput and puts it in "parts"; parts[0], parts[1], etc.
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
        else if (parts[0] == "refresh")
        {
            return (true, "Status Refreshed."); //doesnt really do anything other than refresh the UI and the user will see the status below header
        }
        else if (parts[0] == "device")
        {
            if 
                (
                parts.Length == 4 &&
                parts[1] == "change" &&
                int.TryParse(parts[3], out int deviceNumber)
                )
            {
                List<AudioDevice> devices = _audio.GetOutputDevices();

                if (deviceNumber < 1 || deviceNumber > devices.Count)
                {
                    return (true, "Invalid device. Input the number corresponding to the device.");
                }

                AudioDevice selectedDevice = devices[deviceNumber - 1];
                string deviceId = selectedDevice.Id;

                if (parts[2] == "multimedia")
                {
                    bool successChange = _audio.SetDefaultDevice(
                        deviceId,
                        AudioManager.ERole.eMultimedia);

                    if (successChange)
                    {
                        return (true,
                            $"Default multimedia device changed to {selectedDevice.DeviceFriendlyName}.");
                    }
                    else
                    {
                        return (true,
                            "Unable to change the default multimedia device.");
                    }
                }

                if (parts[2] == "communications")
                {
                    bool successChange = _audio.SetDefaultDevice(
                        deviceId,
                        AudioManager.ERole.eCommunications);

                    if (successChange)
                    {
                        return (true,
                            $"Default communications device changed to {selectedDevice.DeviceFriendlyName}.");
                    }
                    else
                    {
                        return (true,
                            "Unable to change the default communications device.");
                    }
                }
                return (true, "Invalid device role. Use \"multimedia\" or \"communications\".");
            }
            else
            {
                return (true, "Invalid device command. Please type \"help\" for more instructions.");
            }
        }
        else if (parts[0].ToLower() == "sessions")
        {
            var sessions = _audio.GetActiveAudioSessions();

            if (sessions.Count == 0)
            {
                return (true, "No active audio sessions found.");
            }
            else
            {
                // Format the raw tuples back into text for the command result
                List<string> formatted = new List<string>();
                foreach (var s in sessions)
                {
                    formatted.Add($"{s.ProcessName} - Volume: {Math.Round(s.Volume)}%");
                }
                string sessionList = string.Join("\n", formatted);
                return (true, "Active Audio Sessions:\n" + sessionList);
            }
        }
        else if (parts[0] == "session")
        {
            if (parts.Length == 4 && parts[2] == "volume")
            {
                string targetApp = parts[1];

                // Using int.TryParse to keep it consistent with your Master Volume logic
                if (int.TryParse(parts[3], out int targetVolume))
                {
                    _audio.SetApplicationVolume(targetApp, targetVolume);
                    return (true, $"Changed {targetApp} volume to {targetVolume}%");
                }
                else
                {
                    return (true, "Unable to change session volume. Input a valid number.");
                }
            }
            else
            {
                return (true, "Invalid session command. Example: session chrome volume 50");
            }
        }
        return (true, _ui.GetErrorInvalidCommand());
    }
}