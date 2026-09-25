using System.Reflection;
using ArkBase.Characters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using STS2RitsuLib;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace ArkBase;

[ModInitializer(nameof(Initialize))]
public static class Entry
{
    public const string ModId = "ArkBase";
    public const string ResPath = $"res://{ModId}";

    public static Logger Logger { get; } = new(ModId, LogType.Generic);

    public static void Initialize()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        RitsuLibFramework.EnsureGodotScriptsRegistered(assembly, Logger);
        ModTypeDiscoveryHub.RegisterModAssembly(ModId, assembly);

        ModContentRegistry.For(ModId).RegisterCardLibraryCompendiumSharedPoolFilter<ArkSupportCardPool>(
            "SUPPORT",
            $"{ResPath}/images/card_pools/support.png",
            [
                new CardLibraryCompendiumPlacementRule
                {
                    VanillaFilterAnchorUniqueName = "ColorlessPool",
                    Relation = CardLibraryCompendiumFilterInsertRelation.Before
                }
            ]);

        new Harmony(ModId).PatchAll(assembly);
        Logger.Info("ArkBase initialized.");
    }
}
