using ArkBase.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ArkBase.Api;

public static class StatusEffects
{
    public static Task<SluggishPower?> ApplySluggish(
        PlayerChoiceContext context, Creature target, int stacks, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<SluggishPower>(context, target, stacks, applier, source);

    public static Task<ShiverPower?> ApplyShiver(
        PlayerChoiceContext context, Creature target, int turns, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<ShiverPower>(context, target, turns, applier, source);

    public static Task<ParalysisPower?> ApplyParalysis(
        PlayerChoiceContext context, Creature target, int stacks, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<ParalysisPower>(context, target, stacks, applier, source);

    public static Task<SilencePower?> ApplySilence(
        PlayerChoiceContext context, Creature target, int stacks, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<SilencePower>(context, target, stacks, applier, source);

    public static Task Stun(
        PlayerChoiceContext context, Creature target, Creature applier,
        CardModel? source = null) =>
        SluggishPower.ClearSluggishAndStun(context, target, applier, source);
}
