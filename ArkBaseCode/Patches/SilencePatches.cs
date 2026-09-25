using System.Reflection;
using ArkBase.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;

namespace ArkBase.Patches;

[HarmonyPatch(typeof(PowerCmd), nameof(PowerCmd.Apply),
    [typeof(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext),
     typeof(PowerModel), typeof(Creature), typeof(decimal), typeof(Creature),
     typeof(CardModel), typeof(bool)])]
internal static class SilencePowerApplyPatch
{
    private static bool Prefix(Creature target, Creature? applier, ref Task __result)
    {
        if (!SilencePower.IsActing(applier) &&
            !(applier is null && SilencePower.IsActingIn(target.CombatState)))
        {
            return true;
        }

        __result = Task.CompletedTask;
        return false;
    }
}

[HarmonyPatch]
internal static class SilenceGainBlockPatch
{
    private static IEnumerable<MethodBase> TargetMethods() =>
        AccessTools.GetDeclaredMethods(typeof(CreatureCmd))
            .Where(method => method.Name == nameof(CreatureCmd.GainBlock));

    private static bool Prefix(Creature creature, ref Task<decimal> __result)
    {
        if (!SilencePower.IsActing(creature))
        {
            return true;
        }

        __result = Task.FromResult(0m);
        return false;
    }
}

[HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.Heal))]
internal static class SilenceHealPatch
{
    private static bool Prefix(Creature creature, ref Task __result)
    {
        if (!SilencePower.IsActing(creature))
        {
            return true;
        }

        __result = Task.CompletedTask;
        return false;
    }
}

[HarmonyPatch]
internal static class SilenceAddCardPatch
{
    private static IEnumerable<MethodBase> TargetMethods() =>
        AccessTools.GetDeclaredMethods(typeof(CardPileCmd))
            .Where(method => method.Name == nameof(CardPileCmd.Add) &&
                method.GetParameters().Length == 5 &&
                method.GetParameters()[0].ParameterType == typeof(CardModel));

    private static bool Prefix(CardModel card, AbstractModel? clonedBy,
        ref Task<CardPileAddResult> __result)
    {
        // Existing cards must still move between piles after an enemy attack.
        if (card.Pile is not null ||
            !(clonedBy is MonsterModel monster && SilencePower.IsActing(monster.Creature)) &&
            !SilencePower.IsActingIn(card.CombatState ?? card.Owner?.Creature.CombatState))
        {
            return true;
        }

        __result = Task.FromResult(default(CardPileAddResult));
        return false;
    }
}

[HarmonyPatch(typeof(CardPileCmd), nameof(CardPileCmd.AddGeneratedCardsToCombat))]
internal static class SilenceGenerateCardsPatch
{
    private static bool Prefix(Player creator,
        ref Task<IReadOnlyList<CardPileAddResult>> __result)
    {
        if (!SilencePower.IsActingIn(creator.Creature.CombatState))
        {
            return true;
        }

        __result = Task.FromResult<IReadOnlyList<CardPileAddResult>>([]);
        return false;
    }
}

[HarmonyPatch(typeof(CardCmd), nameof(CardCmd.Afflict),
    [typeof(AfflictionModel), typeof(CardModel), typeof(decimal)])]
internal static class SilenceAfflictCardPatch
{
    private static bool Prefix(AfflictionModel affliction, CardModel card,
        ref Task<AfflictionModel> __result)
    {
        if (!SilencePower.IsActingIn(card.CombatState ?? card.Owner?.Creature.CombatState))
        {
            return true;
        }

        __result = Task.FromResult(affliction);
        return false;
    }
}

[HarmonyPatch(typeof(CardCmd), nameof(CardCmd.ApplyKeyword))]
internal static class SilenceApplyCardKeywordPatch
{
    private static bool Prefix(CardModel card) =>
        !SilencePower.IsActingIn(card.CombatState ?? card.Owner?.Creature.CombatState);
}

[HarmonyPatch]
internal static class SilenceStunPatch
{
    private static IEnumerable<MethodBase> TargetMethods() =>
        AccessTools.GetDeclaredMethods(typeof(CreatureCmd))
            .Where(method => method.Name == nameof(CreatureCmd.Stun));

    private static bool Prefix(Creature creature, ref Task __result)
    {
        if (!SilencePower.IsActingIn(creature.CombatState))
        {
            return true;
        }

        __result = Task.CompletedTask;
        return false;
    }
}

[HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.Add),
    [typeof(Creature)])]
internal static class SilenceSummonPatch
{
    private static bool Prefix(Creature creature, ref Task __result)
    {
        if (!SilencePower.IsActingIn(creature.CombatState))
        {
            return true;
        }

        __result = Task.CompletedTask;
        return false;
    }
}

[HarmonyPatch(typeof(PlayerCmd), nameof(PlayerCmd.LoseEnergy))]
internal static class SilenceLoseEnergyPatch
{
    private static bool Prefix(Player player, ref Task __result)
    {
        if (!SilencePower.IsActingIn(player.Creature.CombatState))
        {
            return true;
        }

        __result = Task.CompletedTask;
        return false;
    }
}
