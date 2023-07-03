using Gameplay.Systems;
using ProjectM;
using ProjectM.Gameplay.Scripting;
using ProjectM.Network;
using ProjectM.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Wetstone.API;

namespace vrising_stash
{
    public class QuickStashServer
    {
        private static readonly List<PrefabGUID> _itemRefreshGuids = new() {
            new PrefabGUID(1686577386), // Silver Ore
            new PrefabGUID(-949672483)  // Silver Coin
        };

        private static readonly Dictionary<Entity, DateTime> _lastMerge = new();

        public static void OnMergeInventoriesMessage(FromCharacter fromCharacter, MergeInventoriesMessage msg)
        {
            if (!VWorld.IsServer || fromCharacter.Character == Entity.Null)
            {
                return;
            }

            if (_lastMerge.ContainsKey(fromCharacter.Character) && DateTime.Now - _lastMerge[fromCharacter.Character] < TimeSpan.FromSeconds(0.5))
            {
                return;
            }
            _lastMerge[fromCharacter.Character] = DateTime.Now;

            InventoryUtilities.TryGetInventoryEntity(VWorld.Server.EntityManager, fromCharacter.Character, out Entity playerInventory);

            if (playerInventory == Entity.Null)
            {
                return;
            }

            var gameManager = VWorld.Server.GetExistingSystem<ServerScriptMapper>()?._ServerGameManager;
            var gameDataSystem = VWorld.Server.GetExistingSystem<GameDataSystem>();

            var entities = QuickStashShared.GetStashEntities(VWorld.Server.EntityManager);
            foreach (var toEntity in entities)
            {
                if (!gameManager._TeamChecker.IsAllies(fromCharacter.Character, toEntity))
                {
                    continue;
                }

                if (!IsWithinDistance(playerInventory, toEntity, VWorld.Server.EntityManager))
                {
                    continue;
                }

                InventoryUtilitiesServer.TrySmartMergeInventories(VWorld.Server.EntityManager, gameDataSystem.ItemHashLookupMap, playerInventory, toEntity, out _);
            }

            // Refresh silver debuff
            foreach (var prefabGuid in _itemRefreshGuids)
            {
                InventoryUtilitiesServer.CreateInventoryChangedEvent(VWorld.Server.EntityManager, fromCharacter.Character, prefabGuid, 0, InventoryChangedEventType.Moved);
            }
        }

        private static bool IsWithinDistance(Entity interactor, Entity inventory, EntityManager entityManager)
        {
            var interactorLocation = entityManager.GetComponentData<LocalToWorld>(interactor);
            var inventoryLocation = entityManager.GetComponentData<LocalToWorld>(inventory);

            Vector3 difference = new(
                interactorLocation.Position.x - inventoryLocation.Position.x,
                interactorLocation.Position.y - inventoryLocation.Position.y,
                interactorLocation.Position.z - inventoryLocation.Position.z);

            double distance = Math.Sqrt(
                  Math.Pow(difference.x, 2f) +
                  Math.Pow(difference.y, 2f) +
                  Math.Pow(difference.z, 2f));

            if (distance > Plugin.configMaxDistance.Value)
            {
                return false;
            }

            return true;
        }
    }
}
