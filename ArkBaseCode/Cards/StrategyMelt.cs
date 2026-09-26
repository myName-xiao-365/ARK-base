using ArkBase.Characters;
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
public sealed class StrategyMelt : SupportCardTemplate
{
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/StrategyMelt.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar("HpLoss", 8m)
    ];

    public StrategyMelt() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
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

        Dictionary<Creature, int> hpBefore = targets.ToDictionary(target => target, target => target.CurrentHp);
        await CreatureCmd.Damage(
            choiceContext,
            targets,
            DynamicVars["HpLoss"].BaseValue,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move,
            Owner.Creature,
            this);

        int actualHpLost = targets.Sum(target => Math.Max(0, hpBefore[target] - target.CurrentHp));
        if (actualHpLost > 0)
        {
            await CreatureCmd.Heal(Owner.Creature, actualHpLost / 2m);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["HpLoss"].UpgradeValueBy(2m);
    }
}
