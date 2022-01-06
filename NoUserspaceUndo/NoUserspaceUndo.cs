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
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/badhaloninja/NoUserspaceUndo";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("me.badhaloninja.NoUserspaceUndo");
            harmony.PatchAll();
        }
        
        public static bool Check(UndoManager __instance)
        {
            if (!__instance.World.IsUserspace()) return true;

            DynamicVariableSpace space = __instance.World.RootSlot.FindSpace("World");
            if (space == null) return false;

            space.TryReadValue<bool>("UseUndoManager", out bool useUndoManager); 
            return useUndoManager;
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