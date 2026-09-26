using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class FearPower : ModPowerTemplate
{
    public const decimal DamageTakenIncreasePerStack = 0.1m;

    public static decimal GetDamageTakenMultiplierForStacks(int stacks) =>
        1m + Math.Max(0, stacks) * DamageTakenIncreasePerStack;

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/powers/FearPower.png",
        BigIconPath: $"{Entry.ResPath}/images/powers/FearPower.png");

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource) =>
        Amount > 0 && ReferenceEquals(target, Owner)
            ? GetDamageTakenMultiplierForStacks(Amount)
            : 1m;
}
