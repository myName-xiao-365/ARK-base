using ArkBase.Characters;
using ArkBase.Keywords;
using ArkBase.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class Dreadburst : SupportCardTemplate
{
    private Creature? _primaryTarget;

    public override bool ShouldReceiveCombatHooks => IsInCombat;

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, CardKeyword.Exhaust, ArkKeywords.Fear];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/Dreadburst.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12, ValueProp.Move),
        new PowerVar<FearPower>(2)
    ];

    public Dreadburst() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        if (CombatState is not { } combatState)
        {
            return;
        }

        List<Creature> targets = combatState.HittableEnemies.ToList();
        Creature? previousTarget = _primaryTarget;
        _primaryTarget = cardPlay.Target;
        try
        {
            // A single attack hit shares Vigor and consumes Paralysis only once.
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(combatState)
                .Execute(choiceContext);
        }
        finally
        {
            _primaryTarget = previousTarget;
        }

        decimal fear = DynamicVars[nameof(FearPower)].BaseValue;
        foreach (Creature target in targets.Where(target => target.IsAlive))
        {
            await PowerCmd.Apply<FearPower>(
                choiceContext, target,
                ReferenceEquals(target, cardPlay.Target) ? fear : Math.Floor(fear / 2m),
                Owner.Creature, this);
        }
    }

    public override decimal ModifyDamageMultiplicative(
        Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        return _primaryTarget is not null && target is not null &&
               !ReferenceEquals(target, _primaryTarget) && ReferenceEquals(cardSource, this) &&
               ReferenceEquals(dealer, Owner.Creature) && props.IsPoweredAttack()
            ? 0.5m
            : 1m;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4);
        DynamicVars[nameof(FearPower)].UpgradeValueBy(1);
    }
}
