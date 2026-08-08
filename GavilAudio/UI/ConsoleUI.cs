using GavilAudio.Audio;

namespace GavilAudio.UI;

public class ConsoleUI
{
    /// DRAWING METHODS
    
    public void ShowHeader()
    {
        Console.Clear();
        Console.WriteLine("=================================");
        Console.WriteLine("          GavilAudio");
        Console.WriteLine("=================================");
        Console.WriteLine();
    }
    public void ShowCurrentStatus(string currentDevice, int volume)
    {
        Console.WriteLine($"Current Device: {currentDevice}");
        Console.WriteLine();
        Console.WriteLine($"Current Master Volume: {volume}%");
        Console.WriteLine();
        Console.WriteLine("---------------------------------");
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
        Console.WriteLine();
        Console.WriteLine("---------------------------------");
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
        List<string> deviceFriendlyNameList = new List<string>();
        foreach (AudioDevice device in devices)
        {
            deviceFriendlyNameList.Add(device.DeviceFriendlyName);
        }
        return string.Join(", ", deviceFriendlyNameList);
    }
    // When someone calls ShowDeviceList, it must provide an AudioDevice-type list (devices).
    // Then it creates a new string-type list. Goes through the devices list one device at
    // the time. Puts the DeviceFriendlyName string into DeviceFrienlyNameList. Lasty it string.Joint
    // the names to return as one string.

    /// MESSAGE METHODS
    public string ShowHelp()
    {
        return "Set the master volume with \"volume <0-100>\". Example: volume 50. type \"exit\" to close application.";
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