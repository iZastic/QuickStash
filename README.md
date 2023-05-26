# QuickStash

A mod that uses _Compulsively Count_ on all nearby stashes with 1 click.  
Default key is G.

Note this is both a client and server mod.

### Installation

Required

- Install [BepInEx](https://v-rising.thunderstore.io/package/BepInEx/BepInExPack_V_Rising/)
- Install [Wetstone](https://v-rising.thunderstore.io/package/molenzwiebel/Wetstone/)
- Extract _QuickStash.dll_ into _(VRising folder)/BepInEx/plugins_
- Extract _QuickStash.dll_ into _(VRising folder)/VRising_Server/BepInEx/plugins_

Optional

- for singleplayer, install [ServerLaunchFix](https://v-rising.thunderstore.io/package/Mythic/ServerLaunchFix/) to fix issues with the server mod not working

### Configuration

The keybindings are changed in the menu in-game.

For server configuration, after running the game once, the config file will be generated.

- Update server config in _(VRising folder)/VRising_Server/BepInEx/config/QuickStash.cfg_

### Troubleshooting

- If the mod doesn't work, it is probably because the server part is not installed, check your BepInEx logs on both the client and server to make sure you are running the latest version of both QuickStash and Wetstone.

### Support
- Open an [issue](https://github.com/Elmegaard/QuickStash/issues) on [github](https://github.com/Elmegaard/QuickStash)
- Ask in the [V Rising Mod Community](https://discord.gg/CWzkHvekg3) Discord

### Changelog
`1.3.0`
- Upgrade for Gloomrot

<details>

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
