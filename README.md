# Rive Maui

Wrapper around the iOS/Android runtime

_(Work in progress)_

<img src="./ios.gif" width="320"> <img src="./ios2.gif" width="320">
<br>
<img src="./android.gif" width="320"> <img src="./android2.gif" width="320">


## Getting started
- Install Rive.Maui nuget
- Call .UseRive() on MauiAppBuilder in MauiProgram.cs
- Set iOS target version to at least 14 `<SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">14.0</SupportedOSPlatformVersion>`
- Add .riv files to Resources/Images