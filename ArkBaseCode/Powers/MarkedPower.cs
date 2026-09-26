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
public sealed class MarkedPower : ModPowerTemplate
{
    public const int DamagePerStack = 5;

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/powers/MarkedPower.png",
        BigIconPath: $"{Entry.ResPath}/images/powers/MarkedPower.png");

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Player || Owner.Side != CombatSide.Enemy ||
            !Owner.IsAlive || Amount <= 0 || !Owner.Powers.Contains(this))
        {
            return;
        }

        Flash();
        var target = Owner;
        var applier = Applier;
        int hits = Amount;
        for (int hit = 0; hit < hits && target.IsAlive; hit++)
        {
            await CreatureCmd.Damage(
                choiceContext, target, DamagePerStack, ValueProp.Unpowered, applier, null);
        }

        if (target.Powers.Contains(this))
        {
            await PowerCmd.Decrement(this);
        }
    }
}
