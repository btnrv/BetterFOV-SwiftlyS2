<div align="center">
  <img src="https://pan.samyyc.dev/s/VYmMXE" />
  <h2><strong>BetterFOV</strong></h2>
  <h3>Configurable FOV manager with Cookies persistence</h3>
</div>

## Features

- Allow players to customize their FOV within configurable limits
- Persistent FOV preferences using Cookies plugin integration
- Simple command-based interface
- Configurable FOV ranges and defaults

## Installation

1. Install the [Cookies plugin](https://github.com/SwiftlyS2-Plugins/Cookies)
2. Extract the BetterFOV.zip to your SwiftlyS2 plugins directory
3. Configure the plugin in `configs/plugins/BetterFOV/config.jsonc`
4. Restart your server

## Configuration

Edit `configs/plugins/BetterFOV/config.jsonc`:

```jsonc
{
  "BetterFOV": {
    "Enabled": true,
    "EnableCookiesCaching": true,
    "Prefix": "[red][BetterFOV][default] ",
    "MinFOV": 80,
    "MaxFOV": 130,
    "DefaultFOV": 90,
    "CookieKey": "betterfov",
    "Commands": ["fov"]
  }
}
```

- `EnableCookiesCaching: true` queues FOV cookie writes and flushes them on round end.
- `EnableCookiesCaching: false` saves to Cookies immediately when command is used.

## Usage

Players can use the following command:
- `!fov [value]` - Set FOV to specified value (or reset to default if no value provided)

Example:
- `!fov 110` - Sets FOV to 110
- `!fov` - Resets FOV to default value

## Building

1. **Clone the repository with submodules**
   ```bash
   git clone --recurse-submodules https://github.com/btnrv/BetterFOV.git
   cd BetterFOV/BetterFOV
   ```

2. **Build the plugin**
   ```bash
   dotnet publish BetterFOV.csproj
   ```

The output will be in `build/publish/BetterFOV/` and a zip file will be created at `BetterFOV.zip`.
