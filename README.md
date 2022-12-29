 ![GitHub](https://img.shields.io/github/license/shells-dw/loupedeck-pihole)
 ![GitHub last commit](https://img.shields.io/github/last-commit/shells-dw/loupedeck-pihole)
  ![GitHub downloads](https://img.shields.io/github/downloads/shells-dw/loupedeck-pihole/total)
 [![Tip](https://img.shields.io/badge/Donate-PayPal-green.svg)]( https://www.paypal.com/donate?hosted_button_id=8KXD334CCEEC2) / [![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y4CE9LH)


# Unofficial Loupedeck PiHole Plugin

![Overview1](/PiholePlugin/docs/images/overview.png)

## What Is This (and what does it do?)

It's a plugin for the [Loupedeck Live Consoles][Loupedeck] that uses the official Pi-Hole API to enable and disable the blocking as well as display a lot of statistics on the device.
Disabling your ad blocking is just a touch away now ;)

**Works on Windows and MacOS**

_Note: I'm not affiliated with Pi-Hole, this plugin just uses the official API to control it from a Loupedeck._

## Release / Installation

You can find the precompiled plugin lplug4 file in the [Releases][Releases]. Download and open it, your computer should already recognize this as a Loupedeck plugin file and offer to open it with Loupedeck Configuration Console - which will have the plugin available in the list then.

## Setup

- The plugin defaults to http://pi.hole/admin/api.php with no API token. If you don't use authentication for your Pi-Hole you'll be fine.

- If you use authentication, get an API key from the Pi-Hole Admin Console -> Settings -> API / Web interface -> Show API token and paste it in the ApiToken-Field of settings.json (see below)

## settings.json
**Windows**: %localappdata%\Loupedeck\PluginData\Pihole

**MacOS**: /Users/USERNAME/.local/share/Loupedeck/PluginData/Pihole

contains the file settings.json (which is created with default values during the first start of the plugin and read during every start to allow updating settings)


```json
{
  "ApiUrl": "http://pi.hole/admin/api.php",
  "ApiToken": "[securely stored in plugin settings - replace with new token if neccessary]"
}
```
Loupedeck offers a secure way to store plugin settings, so the plugin reads the "ApiToken" during startup and removes the Token from the settings.json file after storing it.

If you need to change the token, simply replace the value of "ApiToken" with the actual token, restart Loupedeck and the plugin will update the token and remove it from the file.

## Usage
### General

You have the following options:

![Available Actions](/PiholePlugin/docs/images/actions.png)

Which is basically everything the Pi-Hole API offers.

## Actions

### Enable

- Enables blocking (turns grey with a green layer below the text if blocking is disabled to visually advise the current blocking status and make it more obvious where to reenable the filtering).

### Disable

- Disables blocking (turns grey of blocking is disabled to visually advise the current blocking status).

### Display

- Obviously these are just informative displays, there is no touch action behind those, Display actions are just to view current stats.


# I have an issue or miss a feature

You can submit an issue or request a feature with [GitHub issues]. Please describe as good as possible what went wrong or doesn't act like you'd expect it to. 

# Support

If you'd like to drop me a coffee for the hours I've spent on this:
[![Tip](https://img.shields.io/badge/Donate-PayPal-green.svg)]( https://www.paypal.com/donate?hosted_button_id=8KXD334CCEEC2)
or use Ko-Fi [![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y4CE9LH)

# Changelog
## [1.2.0] - 2022-12-29
### Added
- Logging

<details><summary>Changelog History</summary><p>

## [1.1.0] - 2022-12-22
### Improved
- process flow
## [1.0.0] - 2022-11-26
### Added
Initial release

</p></details>


<!-- Reference Links -->

[Loupedeck]: https://loupedeck.com "Loupedeck.com"
[Releases]: https://github.com/shells-dw/loupedeck-pihole/releases "Releases"
[PiHole]: https://pi-hole.net "Pi-hole® Network-wide Ad Blocking ﻿"
[GitHub issues]: https://github.com/shells-dw/loupedeck-pihole/issues "GitHub issues link"

