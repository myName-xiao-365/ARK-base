using System.Threading;
using ArkBase.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace ArkBase.Patches;

[HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.Heal))]
internal static class ReconstructionBodyHealingPatch
{
    private readonly record struct HealState(
        Creature[]? Teammates,
        decimal AmountToShare,
        int HpBeforeHealing);

    private static readonly AsyncLocal<bool> IsSharingHeal = new();

    private static void Prefix(
        Creature creature,
        ref decimal amount,
        out HealState __state)
    {
        __state = default;
        if (IsSharingHeal.Value || !creature.IsPlayer || amount <= 0 ||
            !creature.Powers.OfType<ReconstructionBodyPower>().Any(power => power.Amount > 0))
        {
            return;
        }

        Creature[] teammates = (creature.CombatState as CombatState)?.Players
            .Select(player => player.Creature)
            .Where(teammate => !ReferenceEquals(teammate, creature) && teammate.IsAlive)
            .ToArray() ?? [];

        if (teammates.Length == 0)
        {
            amount *= 1.5m;
            return;
        }

        __state = new HealState(teammates, amount, creature.CurrentHp);
    }

    private static async Task Postfix(
        Task __result,
        Creature creature,
        HealState __state)
    {
        await __result;
        if (IsSharingHeal.Value || __state.Teammates is not { Length: > 0 } teammates ||
            creature.CurrentHp <= __state.HpBeforeHealing)
        {
            return;
        }

        IsSharingHeal.Value = true;
        try
        {
            foreach (Creature teammate in teammates.Where(teammate => teammate.IsAlive))
            {
                await CreatureCmd.Heal(teammate, __state.AmountToShare / 2m);
            }
        }
        finally
        {
            IsSharingHeal.Value = false;
        }
    }
}
