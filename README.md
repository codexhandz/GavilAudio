# GavilAudio

A lightweight Windows console application for monitoring and controlling audio devices.

GavilAudio is a personal C# learning project. It was built as a way to learn software development by creating a useful application rather than following tutorials alone.

## Features

- View the current master volume
- Change master volume from 0–100%
- Detect available Windows playback devices
- Identify the default Multimedia and Communications devices
- Change the default Multimedia device
- Change the default Communications device
- Refresh the audio and device information
- Simple command-line interface
- Input validation and error handling

## Commands

```text
exit
help
volume <0-100>
status
refresh
device change <multimedia|communications> <device number>
```

### Examples

```text
volume 50
```

```text
device change multimedia 2
```

## Technology

- C#
- .NET 8
- NAudio
- Windows Core Audio
- Windows COM / IPolicyConfig

## Project Status

**Version 1 — Complete**

The first version focuses on the core functionality of interacting with the Windows audio system.

Future versions may explore features such as:

- Mute/unmute
- Per-application audio sessions
- Individual application volume control
- User preferences and configuration
- Additional UI options
- Rainmeter integration

These are ideas for future development, not a fixed roadmap.

## Learning Project & AI Assistance

GavilAudio is primarily a learning project, and the code is expected to contain areas that can be improved as I continue learning.

I used **OpenAI's ChatGPT** as a development and learning assistant throughout the project. ChatGPT helped explain C# concepts, troubleshoot errors, explore architectural decisions, research Windows audio APIs, and guide me through unfamiliar concepts such as COM interop and `IPolicyConfig`.

The implementation, testing, decisions, and direction of the project remain my own. ChatGPT was used as a tool to help me understand and build the project.

## Disclaimer

GavilAudio is **not production-grade software** and has not been extensively tested across different Windows versions or hardware configurations.

The application interacts directly with the Windows audio system and uses the Windows `IPolicyConfig` COM interface for device switching. This is a lower-level Windows interface and may change or stop working with future Windows updates.

Use GavilAudio at your own discretion. No guarantee is made that it will work correctly on every system.

## Feedback

This repository is public as part of my learning journey.

Constructive feedback is welcome, especially feedback that explains **why** something could be improved or suggests a better approach.
