using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.AttackHits;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class OriginiumPower : ModPowerTemplate, IAttackHitHookListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/powers/OriginiumPower.png",
        BigIconPath: $"{Entry.ResPath}/images/powers/OriginiumPower.png");

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        return Amount > 0 && ReferenceEquals(dealer, Owner) && props.IsPoweredAttack()
            ? 2m
            : 1m;
    }

    public async Task AfterAttackHit(AttackHitContext context)
    {
        if (Amount <= 0 || !Owner.IsAlive ||
            !ReferenceEquals(context.Dealer, Owner) ||
            !context.DamageProps.IsPoweredAttack() ||
            context.Targets.Count == 0)
        {
            return;
        }

        Flash();
        await CreatureCmd.Damage(
            context.ChoiceContext,
            Owner,
            2m,
            ValueProp.Unpowered | ValueProp.SkipHurtAnim,
            Owner);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner))
        {
            await PowerCmd.Remove(this);
        }
    }
}
