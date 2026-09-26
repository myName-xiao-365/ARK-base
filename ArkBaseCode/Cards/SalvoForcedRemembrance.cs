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
public sealed class SalvoForcedRemembrance : SupportCardTemplate
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, CardKeyword.Exhaust, ArkKeywords.Marked];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/SalvoForcedRemembrance.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<MarkedPower>(6)
    ];

    public SalvoForcedRemembrance() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
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

        int minimumMarked = enemies.Min(enemy =>
            enemy.Powers.OfType<MarkedPower>().FirstOrDefault()?.Amount ?? 0);
        var candidates = enemies
            .Where(enemy => (enemy.Powers.OfType<MarkedPower>().FirstOrDefault()?.Amount ?? 0) == minimumMarked)
            .ToArray();
        var target = combatState.RunState.Rng.CombatTargets.NextItem(candidates);
        if (target is null)
        {
            return;
        }

        await PowerCmd.Apply<MarkedPower>(
            choiceContext, target, DynamicVars[nameof(MarkedPower)].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => DynamicVars[nameof(MarkedPower)].UpgradeValueBy(2);
}
