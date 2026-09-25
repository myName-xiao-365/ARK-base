using ArkBase.Characters;
using ArkBase.Keywords;
using ArkBase.Powers;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class HeadWolf : SupportCardTemplate
{
    internal const int CooldownAttacks = 3;

    private int _stage;

    public override bool ShouldReceiveCombatHooks => IsInCombat;

    public override IEnumerable<CardKeyword> CanonicalKeywords => _stage == 0
        ? [CardKeyword.Retain, CardKeyword.Exhaust]
        : [CardKeyword.Retain, CardKeyword.Exhaust, ArkKeywords.Silence];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/{GetType().Name}.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12, ValueProp.Move),
        new DamageVar("LaterDamage", 15, ValueProp.Move)
    ];

    public HeadWolf() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState is not { } combatState)
        {
            return;
        }

        List<Creature> targets = combatState.HittableEnemies.ToList();
        decimal damage = _stage == 0
            ? DynamicVars.Damage.BaseValue
            : DynamicVars["LaterDamage"].BaseValue;

        await DamageCmd.Attack(damage)
            .WithHitCount(_stage == 3 ? 2 : 1)
            .FromCard(this)
            .TargetingAllOpponents(combatState)
            .Execute(choiceContext);

        if (_stage >= 2)
        {
            foreach (Creature target in targets.Where(target => target.IsAlive))
            {
                await PowerCmd.Apply<SilencePower>(
                    choiceContext,
                    target,
                    1,
                    Owner.Creature,
                    this);
            }
        }

        if (_stage < 3)
        {
            _stage++;
            this.RequestVisualReload();
        }
    }

    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (!ReferenceEquals(card, this))
        {
            return;
        }

        HeadWolfCooldownPower? cooldown = Owner.Creature.Powers
            .OfType<HeadWolfCooldownPower>()
            .FirstOrDefault();
        cooldown ??= await PowerCmd.Apply<HeadWolfCooldownPower>(
            choiceContext,
            Owner.Creature,
            CooldownAttacks,
            Owner.Creature,
            this);
        cooldown?.Track(this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars["LaterDamage"].UpgradeValueBy(3);
    }

    public int Stage => _stage;
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.Description), MethodType.Getter)]
internal static class HeadWolfDescriptionPatch
{
    private static void Postfix(CardModel __instance, ref LocString __result)
    {
        if (__instance is not HeadWolf headWolf)
        {
            return;
        }

        string key = headWolf.Stage switch
        {
            1 => "ARK_BASE_CARD_HEAD_WOLF.descriptionStage2",
            2 => "ARK_BASE_CARD_HEAD_WOLF.descriptionStage3",
            3 => "ARK_BASE_CARD_HEAD_WOLF.descriptionFinal",
            _ => "ARK_BASE_CARD_HEAD_WOLF.description"
        };
        __result = new LocString("cards", key);
    }
}
