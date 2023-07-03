using HarmonyLib;
using ProjectM;

namespace vrising_stash
{
    [HarmonyPatch]
    public class GameplayInputSystem_Patch
    {

        [HarmonyPatch(typeof(GameplayInputSystem), nameof(GameplayInputSystem.HandleInput))]
        [HarmonyPostfix]
        static void HandleInput(GameplayInputSystem __instance, InputState inputState)
        {
            QuickStashClient.HandleInput(__instance);
        }
    }
}
