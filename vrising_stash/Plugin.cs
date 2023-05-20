using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using System.IO;
using Unity.Entities;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using VampireCommandFramework;
//using Wetstone.API;

namespace vrising_stash
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("gg.deca.VampireCommandFramework")]
    //[BepInDependency("xyz.molenzwiebel.wetstone")]
    //[Reloadable]
    public class Plugin : BasePlugin
    {
        public static ManualLogSource Logger;

        //public static Keybinding configKeybinding;
        public static ConfigEntry<float> configMaxDistance;
        private Harmony _hooks;


        private void InitConfig()
        {
            configMaxDistance = Config.Bind("Server", "MaxDistance", 50.0f, "The max distance for transfering items. 5 'distance' is about 1 tile.");

        }
        /*
        private void RegisterMessages()
        {
            VNetworkRegistry.RegisterServerboundStruct<MergeInventoriesMessage>((fromCharacter, msg) =>
            {
                QuickStashServer.OnMergeInventoriesMessage(fromCharacter, msg);
            });
        }

        private void ClearMessages()
        {
            VNetworkRegistry.UnregisterStruct<MergeInventoriesMessage>();
        }*/

        public override void Load()
        {
            Logger = Log;
            InitConfig();

            _hooks = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
            CommandRegistry.RegisterAll();

            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private static World _serverWorld;
        public static World Server
        {
            get
            {
                if (_serverWorld != null) return _serverWorld;

                _serverWorld = GetWorld("Server")
                    ?? throw new System.Exception("There is no Server world (yet). Did you install a server mod on the client?");
                return _serverWorld;
            }
        }

        private static World GetWorld(string name)
        {
            foreach (var world in World.s_AllWorlds)
            {
                if (world.Name == name)
                {
                    return world;
                }
            }

            return null;
        }

        public override bool Unload()
        {
            Config.Clear();
            _hooks.UnpatchSelf();
            return true;
        }
    }
}
