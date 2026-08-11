using GavilAudio.Audio;
using System.Security.Cryptography.X509Certificates;

namespace GavilAudio.UI;

public class ConsoleUI
{
    /// DRAWING METHODS
    public void ShowConsoleUI(int volume, List<AudioDevice> devices)
    {
        // HEADERR
        Console.Clear();
        Console.WriteLine(@" _______  _______  __   __  ___   ___      _______  __   __  ______   ___   _______ 
|       ||   _   ||  | |  ||   | |   |    |   _   ||  | |  ||      | |   | |       |
|    ___||  |_|  ||  |_|  ||   | |   |    |  |_|  ||  | |  ||  _    ||   | |   _   |
|   | __ |       ||       ||   | |   |    |       ||  |_|  || | |   ||   | |  | |  |
|   ||  ||       ||       ||   | |   |___ |       ||       || |_|   ||   | |  |_|  |
|   |_| ||   _   | |     | |   | |       ||   _   ||       ||       ||   | |       |
|_______||__| |__|  |___|  |___| |_______||__| |__||_______||______| |___| |_______|
");
        Console.WriteLine();
        Console.WriteLine($"------------------------------------------------------------------------------------");
        Console.WriteLine();
        // VOLUME
        Console.WriteLine($"Volume: {volume}%");
        Console.WriteLine();
        // DEVICES
        Console.WriteLine($"All available devices:");
        Console.WriteLine();

        int deviceNumber = 1;

        Console.WriteLine($"Devices:");
        Console.WriteLine();

        foreach (AudioDevice device in devices)
        {
            Console.Write($"{deviceNumber}. {device.DeviceFriendlyName} ");

            if (device.IsDefaultForMultimedia)
            {
                Console.Write(" (Default Multimedia)");
            }
            if (device.IsDefaultForCommunications)
            {
                Console.Write(" (Default Communications)");
            }
            Console.WriteLine();
            deviceNumber++;

        }
        Console.WriteLine();
        Console.WriteLine($"------------------------------------------------------------------------------------");
        Console.WriteLine();
        // COMMANDS
        Console.WriteLine(@"Commands:

- exit
- help
- volume <0-100>
- refresh
- device change <multimedia OR communications> <device number>");
        Console.WriteLine();
        Console.WriteLine($"------------------------------------------------------------------------------------");
        Console.WriteLine();
    }
    public void ShowMenu()
    {
        Console.WriteLine($"Commands:");
        Console.WriteLine();
        Console.WriteLine($"exit - volume - help");
        Console.WriteLine($"  status - devices");
        Console.WriteLine();
        Console.WriteLine("---------------------------------");
        Console.WriteLine();
    }
    public void ShowLastInteraction(string userInput, string commandResult)
    {
        Console.WriteLine($"Last Command: " +  userInput);
        Console.WriteLine();
        Console.WriteLine($"Result: " + commandResult);
        Console.WriteLine($"------------------------------------------------------------------------------------");
        Console.WriteLine();
    }
    public string GetCommand()
    {
        Console.Write($"Enter command: ");
        string input = Console.ReadLine();
        return input;
    }
    public string ShowDeviceList(List<AudioDevice> devices)
    {
        int deviceNumber = 1;
            Console.WriteLine($"Devices:");
            Console.WriteLine();
            foreach (AudioDevice device in devices) {
            Console.WriteLine(deviceNumber.ToString() + ". " + device.DeviceFriendlyName);
            deviceNumber++;
        }
        return ("Would you like to change the default multimedia device? (Y/N)");
    }
    // When someone calls ShowDeviceList, it must provide an AudioDevice-type list (devices).
    // Then it creates a new string-type list. Goes through the devices list one device at
    // the time. Puts the DeviceFriendlyName string into DeviceFrienlyNameList. Lasty it string.Joint
    // the names to return as one string.

    /// MESSAGE METHODS
    public string ShowHelp()
    {
        return @"Breakdown of the commands:
1. exit: it will close the application.
2. help: displays current help.
3. volume <0-100>: Set the master volume with volume <0-100>. Example: volume 50.
4. refresh: it will refresh the whole UI, refreshing the stats
5. Type <device change> followed by <type of audio> (multimedia OR communications)
   followed by <device number> you will like to use from: 'All available Devices'.
   Example: device change multimedia 2";
    }
    public string GetErrorNoCommand()
    {
        return "No input detected. Type \"help\" to see available commands.";
    }
    public string GetErrorInvalidVolumeNumber()
    {
        return "Unable to change volume. Input a valid number. Example: volume 50.";
    }
    public string GetErrorInvalidCommand()
    {
        return "Invalid command. Type \"help\" to see available commands.";
    }
}