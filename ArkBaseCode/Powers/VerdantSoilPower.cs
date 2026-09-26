using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using ArkBase.Keywords;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class VerdantSoilPower : ModPowerTemplate
{
    private const decimal HealOnConsume = 4m;

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<string> RegisteredKeywordIds => [ArkKeywords.VerdantSoilId];

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/powers/VerdantSoilPower.png",
        BigIconPath: $"{Entry.ResPath}/images/powers/VerdantSoilPower.png");

    public override decimal ModifyHpLostAfterOstyLate(
        Creature target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return ReferenceEquals(target, Owner) && Amount > 0
            ? 0m
            : amount;
    }

    public override async Task AfterModifyingHpLostAfterOsty()
    {
        if (Amount <= 0)
        {
            return;
        }

        Creature owner = Owner;
        await PowerCmd.Decrement(this);
        await CreatureCmd.Heal(owner, HealOnConsume);
    }
}
