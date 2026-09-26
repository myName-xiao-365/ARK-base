using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class WantedPower : ModPowerTemplate
{
    public const int BountyGold = 30;

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/powers/WantedPower.png",
        BigIconPath: $"{Entry.ResPath}/images/powers/WantedPower.png");

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player || Owner.Side != CombatSide.Enemy ||
            !Owner.IsAlive || Amount <= 0 || !Owner.Powers.Contains(this))
        {
            return;
        }

        var target = Owner;
        var applier = Applier;
        var beneficiary = applier?.Player ?? applier?.PetOwner;
        Flash();
        var results = await CreatureCmd.Damage(
            choiceContext, target, Amount, ValueProp.Unpowered, applier, null);

        // Death can remove the power before the damage command returns.
        if (target.Powers.Contains(this))
        {
            await PowerCmd.Remove(this);
        }

        if (beneficiary is not null && results.Any(result =>
                ReferenceEquals(result.Receiver, target) && result.WasTargetKilled))
        {
            await PlayerCmd.GainGold(BountyGold, beneficiary);
        }
    }
}
