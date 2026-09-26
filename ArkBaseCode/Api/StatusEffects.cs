using ArkBase.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ArkBase.Api;

public static class StatusEffects
{
    public static Task<WantedPower?> ApplyWanted(
        PlayerChoiceContext context, Creature target, int stacks, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<WantedPower>(context, target, stacks, applier, source);

    public static Task<MarkedPower?> ApplyMarked(
        PlayerChoiceContext context, Creature target, int stacks, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<MarkedPower>(context, target, stacks, applier, source);

    public static Task<ServantPower?> ApplyServant(
        PlayerChoiceContext context, Creature target, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<ServantPower>(context, target, 1, applier, source);

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

    public static Task<OriginiumPower?> ApplyOriginium(
        PlayerChoiceContext context, Creature target, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<OriginiumPower>(context, target, 1, applier, source);

    public static Task<FearPower?> ApplyFear(
        PlayerChoiceContext context, Creature target, int stacks, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<FearPower>(context, target, stacks, applier, source);

    public static Task<VerdantSoilPower?> ApplyVerdantSoil(
        PlayerChoiceContext context, Creature target, int stacks, Creature applier,
        CardModel? source = null) =>
        PowerCmd.Apply<VerdantSoilPower>(context, target, stacks, applier, source);

    public static Task Stun(
        PlayerChoiceContext context, Creature target, Creature applier,
        CardModel? source = null) =>
        SluggishPower.ClearSluggishAndStun(context, target, applier, source);
}
