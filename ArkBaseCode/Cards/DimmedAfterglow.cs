using ArkBase.Characters;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class DimmedAfterglow : SupportCardTemplate
{
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/DimmedAfterglow.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("ThresholdPercent", 50)
    ];

    public DimmedAfterglow() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is not { } combatState)
        {
            return;
        }

        decimal thresholdPercent = DynamicVars["ThresholdPercent"].BaseValue;
        foreach (Creature enemy in combatState.HittableEnemies.ToList())
        {
            if (enemy.MaxHp <= 0 || enemy.CurrentHp * 100m >= enemy.MaxHp * thresholdPercent)
            {
                continue;
            }

            await PowerCmd.Apply<VulnerablePower>(
                choiceContext, enemy, 1, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade() => DynamicVars["ThresholdPercent"].UpgradeValueBy(25);
}
