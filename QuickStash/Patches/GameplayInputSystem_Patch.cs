using HarmonyLib;
using ProjectM;

namespace QuickStash
{
    [HarmonyPatch]
    public class GameplayInputSystem_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameplayInputSystem), nameof(GameplayInputSystem.HandleInput))]
        private static void HandleInput(GameplayInputSystem __instance)
        {
            QuickStashClient.HandleInput(__instance);
        }
    }
}
