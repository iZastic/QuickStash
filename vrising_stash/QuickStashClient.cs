using ProjectM;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;
using Wetstone.API;

namespace vrising_stash
{
    public class QuickStashClient
    {
        private static DateTime _lastInventoryTransfer = DateTime.Now;

        public static void Reset()
        {
            _lastInventoryTransfer = DateTime.Now;
        }

        public static void HandleInput(GameplayInputSystem __instance)
        {
            if (!VWorld.IsClient)
            {
                return;
            }

            if ((Input.GetKeyInt(Plugin.configKeybinding.Primary) || Input.GetKeyInt(Plugin.configKeybinding.Secondary)) && DateTime.Now - _lastInventoryTransfer > TimeSpan.FromSeconds(0.5))
            {
                _lastInventoryTransfer = DateTime.Now;
                TransferItems();
            }
        }

        private static void TransferItems()
        {
            VNetwork.SendToServerStruct<MergeInventoriesMessage>(new()
            {
            });
        }
    }
}
