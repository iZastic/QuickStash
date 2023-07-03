using ProjectM;
using ProjectM.CastleBuilding;
using System.Linq;
using UnhollowerRuntimeLib;
using Unity.Collections;
using Unity.Entities;

namespace vrising_stash
{
    public class QuickStashShared
    {
        private static ComponentType[] _containerComponents = null;
        private static ComponentType[] ContainerComponents
        {
            get
            {
                if (_containerComponents == null)
                {
                    _containerComponents = new[] {

                        ComponentType.ReadOnly(Il2CppType.Of<Team>()),
                        ComponentType.ReadOnly(Il2CppType.Of<CastleHeartConnection>()),
                        ComponentType.ReadOnly(Il2CppType.Of<InventoryBuffer>()),
                        ComponentType.ReadOnly(Il2CppType.Of<NameableInteractable>()),
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
