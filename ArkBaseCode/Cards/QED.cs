using ArkBase.Characters;
using ArkBase.Keywords;
using ArkBase.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class QED : SupportCardTemplate
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, CardKeyword.Exhaust, ArkKeywords.Sluggish];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/QED.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move),
        new PowerVar<SluggishPower>(1)
    ];

    public QED() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is not { } combatState)
        {
            return;
        }

        Creature[] targets = combatState.HittableEnemies.ToArray();
        if (targets.Length == 0)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(combatState)
            .Execute(choiceContext);

        await PowerCmd.Apply<QedSluggishEnergyPower>(
            choiceContext,
            Owner.Creature,
            1,
            Owner.Creature,
            this);

        try
        {
            foreach (Creature target in targets.Where(target => target.IsAlive))
            {
                await PowerCmd.Apply<SluggishPower>(
                    choiceContext,
                    target,
                    DynamicVars[nameof(SluggishPower)].BaseValue,
                    Owner.Creature,
                    this);
            }
        }
        finally
        {
            QedSluggishEnergyPower? listener = Owner.Creature.Powers
                .OfType<QedSluggishEnergyPower>()
                .FirstOrDefault();
            if (listener is not null)
            {
                await PowerCmd.Remove(listener);
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}
