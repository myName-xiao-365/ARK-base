using ArkBase.Characters;
using ArkBase.Keywords;
using ArkBase.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class HomecomingInvitation : SupportCardTemplate
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, CardKeyword.Exhaust, ArkKeywords.Wanted];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/HomecomingInvitation.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WantedPower>(9)
    ];

    public HomecomingInvitation() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is not { } combatState)
        {
            return;
        }

        var enemies = combatState.HittableEnemies;
        if (enemies.Count == 0)
        {
            return;
        }

        int minimumWanted = enemies.Min(enemy =>
            enemy.Powers.OfType<WantedPower>().FirstOrDefault()?.Amount ?? 0);
        var candidates = enemies
            .Where(enemy => (enemy.Powers.OfType<WantedPower>().FirstOrDefault()?.Amount ?? 0) == minimumWanted)
            .ToArray();
        var target = combatState.RunState.Rng.CombatTargets.NextItem(candidates);
        if (target is null)
        {
            return;
        }

        await PowerCmd.Apply<WantedPower>(
            choiceContext, target, DynamicVars[nameof(WantedPower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars[nameof(WantedPower)].UpgradeValueBy(3);
}
