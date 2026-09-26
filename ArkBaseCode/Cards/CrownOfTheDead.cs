using ArkBase.Api;
using ArkBase.Characters;
using ArkBase.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Combat.AttackHits;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class CrownOfTheDead : SupportCardTemplate, IAttackHitHookListener
{
    public const int DeathSummonHealth = 5;

    public override bool ShouldReceiveCombatHooks => IsInCombat;

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, CardKeyword.Exhaust, ArkKeywords.Servant];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/CrownOfTheDead.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(7, ValueProp.Move),
        new DynamicVar("HitCount", 3),
        new DynamicVar("SummonHealth", 10)
    ];

    public CrownOfTheDead() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is not { } combatState)
        {
            return;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .WithHitCount((int)DynamicVars["HitCount"].BaseValue)
            .FromCard(this)
            .TargetingAllOpponents(combatState)
            .Execute(choiceContext);

        await SummonAndGrantServant(choiceContext, DynamicVars["SummonHealth"].BaseValue);
    }

    public async Task AfterAttackHit(AttackHitContext context)
    {
        if (!ReferenceEquals(context.CardSource, this))
        {
            return;
        }

        foreach (var result in context.Results)
        {
            if (result.WasTargetKilled && result.Receiver.Monster is not null &&
                result.Receiver.PetOwner is null)
            {
                await SummonAndGrantServant(context.ChoiceContext, DeathSummonHealth);
            }
        }
    }

    private async Task SummonAndGrantServant(PlayerChoiceContext choiceContext, decimal health)
    {
        await OstyCmd.Summon(choiceContext, Owner, health, this);
        await StatusEffects.ApplyServant(choiceContext, Owner.Creature, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["SummonHealth"].UpgradeValueBy(5);
    }
}
