using ProjectM;
using ProjectM.CastleBuilding;
using ProjectM.Network;
using ProjectM.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using VampireCommandFramework;

namespace vrising_stash
{
    public class QuickStashServer
    {
        private static readonly List<PrefabGUID> _itemRefreshGuids = new() {
            new PrefabGUID(1686577386), // Silver Ore
            new PrefabGUID(-949672483)  // Silver Coin
        };

        [Command("stash", "s", description:"Automatically compulsively count on ALL stashes in range")]
        public static void OnMergeInventoriesMessage(ChatCommandContext ctx)
        {
            InventoryUtilities.TryGetInventoryEntity(Plugin.Server.EntityManager, ctx.Event.SenderUserEntity, out Entity playerInventory);
            if (playerInventory == Entity.Null)
            {
                return;
            }

            var gameManager = Plugin.Server.GetExistingSystem<ServerScriptMapper>()?._ServerGameManager;
            var gameDataSystem = Plugin.Server.GetExistingSystem<GameDataSystem>();

            var entities = GetStashEntities(Plugin.Server.EntityManager);
            foreach (var toEntity in entities)
            {
                if (!IsAllies(ctx.Event.SenderUserEntity, toEntity)){
                    continue;
                }

                if (!IsWithinDistance(playerInventory, toEntity, Plugin.Server.EntityManager))
                {
                    continue;
                }

                InventoryUtilitiesServer.TrySmartMergeInventories(Plugin.Server.EntityManager, gameDataSystem.ItemHashLookupMap, playerInventory, toEntity, out _);
            }

            // Refresh silver debuff
            foreach (var prefabGuid in _itemRefreshGuids)
            {
                InventoryUtilitiesServer.CreateInventoryChangedEvent(Plugin.Server.EntityManager, ctx.Event.SenderUserEntity, prefabGuid, 0, InventoryChangedEventType.Moved);
            }
        }

        public static bool IsAllies(Entity a, Entity b)
        {
            //There is definately an actual way, but i dont know how yet with the team checker gone.
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


        private static ComponentType[] _containerComponents = null;
        private static ComponentType[] ContainerComponents
        {
            get
            {
                if (_containerComponents == null)
                {
                    _containerComponents = new[] {

                        ComponentType.ReadOnly<Team>(),
                        ComponentType.ReadOnly<CastleHeartConnection>(),
                        ComponentType.ReadOnly < InventoryBuffer >(),
                        ComponentType.ReadOnly < NameableInteractable >(),
                    };
                }
                return _containerComponents;
            }
        }

        public static NativeArray<Entity> GetStashEntities(EntityManager entityManager)
        {
            var query = entityManager.CreateEntityQuery(ContainerComponents);
            return query.ToEntityArray(Allocator.Temp);
        }

        public static bool IsEntityStash(EntityManager entityManager, Entity entity)
        {
            return !ContainerComponents.Any(x => !entityManager.HasComponent(entity, x));
        }
    }
}
