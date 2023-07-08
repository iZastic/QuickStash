using BepInEx.Unity.IL2CPP;
using ProjectM.Network;
using VampireCommandFramework;

namespace QuickStash
{
    public static class VCFWrapper
    {
        public static bool Enabled
        {
            get
            {
                return IL2CPPChainloader.Instance.Plugins.ContainsKey("gg.deca.VampireCommandFramework");
            }
        }

        public static void RegisterAll()
        {
            CommandRegistry.RegisterAll();
        }


        [Command("stash", shortHand: "cc", description: "Compulsively Count on all nearby allied chests.", adminOnly: false)]
        public static void VCFCompulsivelyCount(ChatCommandContext ctx)
        {
            var fromChar = new FromCharacter() { User = ctx.Event.SenderUserEntity, Character = ctx.Event.SenderCharacterEntity };

            if (QuickStashServer.MergeInventories(fromChar))
                ctx.Reply("Stashed all items");
            else
                ctx.Reply("Failed to stash items. Contact admin or moderator for support.");
        }
    }
}
