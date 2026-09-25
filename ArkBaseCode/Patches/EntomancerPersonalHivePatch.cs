using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;

namespace ArkBase.Patches;

[HarmonyPatch]
internal static class EntomancerPersonalHivePatch
{
    private static readonly MethodInfo? CastSfxGetter =
        AccessTools.PropertyGetter(typeof(MonsterModel), "CastSfx");

    private static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(Entomancer), "SpitMove");
    }

    private static bool Prefix(Entomancer __instance, ref Task __result)
    {
        __result = SpitWithPersonalHive(__instance);
        return false;
    }

    private static async Task SpitWithPersonalHive(Entomancer entomancer)
    {
        string? castSfx = CastSfxGetter?.Invoke(entomancer, null) as string;
        if (!string.IsNullOrEmpty(castSfx))
        {
            SfxCmd.Play(castSfx, 1f);
        }

        var creature = entomancer.Creature;
        await CreatureCmd.TriggerAnim(creature, "Cast", 0.5f);

        int hiveBefore = creature.Powers.OfType<PersonalHivePower>().FirstOrDefault()?.Amount ?? 0;
        var choiceContext = new ThrowingPlayerChoiceContext();
        await PowerCmd.Apply<PersonalHivePower>(choiceContext, creature, 1, creature, null);
        await PowerCmd.Apply<StrengthPower>(choiceContext, creature, hiveBefore < 3 ? 1 : 2, creature, null);
    }
}
