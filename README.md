# Rive Maui

Wrapper around the iOS/Android runtime

_(Work in progress)_

<img src="./images/ios.gif" width="180"> <img src="./images/ios2.gif" width="180">
<br>
<br>
<img src="./images/android.gif" width="180"> <img src="./images/android2.gif" width="180">


## Getting started
- Call .UseRive() on MauiAppBuilder in MauiProgram.cs
- Set iOS target version in .csproj to at least 14:

```<SupportedOSPlatformVersion Condition="$([MSBuild]::GetTargetPlatformIdentifier('$(TargetFramework)')) == 'ios'">14.0</SupportedOSPlatformVersion>```

- Add .riv files to Resources/Images