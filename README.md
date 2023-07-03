# QuickStash

A fork of the original QuickStash which uses _Compulsively Count_ on all nearby stashes with 1 click.  
Default key is G.

Note this is both a client and server mod.

### Installation
- Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising)
- Install [Bloodstone](https://v-rising.thunderstore.io/package/deca/Bloodstone)
- Extract _QuickStash.dll_ into _(VRising folder)/BepInEx/plugins_
- Extract _QuickStash.dll_ into _(VRising folder)/VRising_Server/BepInEx/plugins_

#### Optional
Singleplayer requires [ServerLaunchFix](https://v-rising.thunderstore.io/package/Mythic/ServerLaunchFix/) to fix issues with the server mods not working.

### Configuration

The keybinding can be changed in the in-game controls menu.

For server configuration, after running the game once, the config file will be generated.

- Update server config in _(VRising folder)/VRising_Server/BepInEx/config/QuickStash.cfg_

### Troubleshooting

- If the mod doesn't work, it may be because it is not installed on the server. Check your BepInEx logs on both the client and server to make sure the latest version of both QuickStash and Bloodstone where loaded.

### Support
- Open an issue on [github](https://github.com/iZastic/QuickStash/issues)
- Ask in the V Rising Mod Community [discord](https://vrisingmods.com/discord)

### Contributors
- iZastic: `@iZastic#0365` on Discord
- Elmegaard: `Elmegaard#4316` on Discord

### Changelog
`1.3.2`
- Moved from Wetstone to Bloodstone

<details>

`1.3.1`
- Added support for bags

`1.3.0`
- Upgrade for Gloomrot

`1.2.3`
- Upgrade to Wetstone 1.1.0
- Potentially fixed rare client crash
- Fixed silver debuff not getting removed

`1.2.2`
- Reduce cooldown from 2 seconds to 0.5 seconds

`1.2.1`
- Fixed Readme

`1.2.0`
- Increased default range to 50
- Added Wetstone (keybinds added to controls in-game)
- Code refactor
- Fixed memory leak (but added small stutter when depositing)

`1.1.2`
- Fixed a client crash

`1.1.1`
- Updated Readme

`1.1.0`
- Set max distance
- Made config for keybind
- Made config for max distance

`1.0.1`
- Updated Readme

`1.0.0`
- Initial mod upload

</details>
