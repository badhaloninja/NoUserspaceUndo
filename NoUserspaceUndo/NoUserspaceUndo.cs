using FrooxEngine;
using FrooxEngine.Undo;
using HarmonyLib;
using NeosModLoader;

namespace NoUserspaceUndo
{
    public class NoUserspaceUndo : NeosMod
    {
        public override string Name => "NoUserspaceUndo";
        public override string Author => "badhaloninja";
        public override string Version => "1.1.1";
        public override string Link => "https://github.com/badhaloninja/NoUserspaceUndo";

        private static ModConfiguration config;

        public override void OnEngineInit()
        {
            config = GetConfiguration();

            Harmony harmony = new Harmony("me.badhaloninja.NoUserspaceUndo");
            harmony.PatchAll();
        }



        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> AllowUserspaceUndo = new ModConfigurationKey<bool>("allowUserspaceUndo", "Allow using undo in userspace", () => false);


        public static bool Check(UndoManager instance)
        {
            if (!instance.World.IsUserspace()) return true;
            return config.GetValue(AllowUserspaceUndo);
        }
        class UndoManager_Patch
        {
            [HarmonyPatch(typeof(UndoManager), "Undo")]
            class Undo_Patch
            {
                public static bool Prefix(UndoManager __instance) => Check(__instance);
            }
            [HarmonyPatch(typeof(UndoManager), "Redo")]
            class Redo_Patch
            {
                public static bool Prefix(UndoManager __instance) => Check(__instance);
            }
        }
    }
}