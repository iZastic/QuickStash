using ProjectM;
using ProjectM.Network;
using ProjectM.Scripting;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Bloodstone.API;

namespace QuickStash
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
            MergeInventories(fromCharacter);
        }

        internal static bool MergeInventories(FromCharacter fromCharacter)
        {

            if (!VWorld.IsServer || fromCharacter.Character == Entity.Null)
            {
                return false;
            }

            if (_lastMerge.ContainsKey(fromCharacter.Character) && DateTime.Now - _lastMerge[fromCharacter.Character] < TimeSpan.FromSeconds(0.5))
            {
                return false;
            }
            _lastMerge[fromCharacter.Character] = DateTime.Now;

            var inventoryEntities = new NativeList<Entity>(Allocator.Temp);
            InventoryUtilities.TryGetInventoryEntities(VWorld.Server.EntityManager, fromCharacter.Character, ref inventoryEntities);

            if (inventoryEntities.Length == 0)
            {
                inventoryEntities.Dispose();
                return false;
            }

            var gameManager = VWorld.Server.GetExistingSystem<ServerScriptMapper>()._ServerGameManager;
            var gameDataSystem = VWorld.Server.GetExistingSystem<GameDataSystem>();

            var stashEntities = QuickStashShared.GetStashEntities(VWorld.Server.EntityManager);
            foreach (var stashEntity in stashEntities)
            {
                if (!gameManager!.IsAllies(fromCharacter.Character, stashEntity))
                {
                    continue;
                }

                if (!IsWithinDistance(fromCharacter.Character, stashEntity, VWorld.Server.EntityManager))
                {
                    continue;
                }

                foreach (var inventoryEntity in inventoryEntities)
                {
                    InventoryUtilitiesServer.TrySmartMergeInventories(VWorld.Server.EntityManager, gameDataSystem.ItemHashLookupMap, inventoryEntity, stashEntity, out _);
                }
            }
            inventoryEntities.Dispose();

            // Refresh silver debuff
            foreach (var prefabGuid in _itemRefreshGuids)
            {
                InventoryUtilitiesServer.CreateInventoryChangedEvent(VWorld.Server.EntityManager, fromCharacter.Character, prefabGuid, 0, InventoryChangedEventType.Moved);
            }

            return true;
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
