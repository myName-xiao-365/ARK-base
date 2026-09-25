using ArkBase.Characters;
using ArkBase.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class Yinyun : SupportCardTemplate
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, CardKeyword.Exhaust, ArkKeywords.Regen];

    private const int BaseEnergyCost = 1;
    private const CardType CardKind = CardType.Skill;
    private const CardRarity CardRarityValue = CardRarity.Rare;
    private const TargetType CardTarget = TargetType.Self;

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/Yinyun.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(8m),
        new PowerVar<RegenPower>(4m),
        new DynamicVar("MaxHp", 3)
    ];

    public Yinyun() : base(BaseEnergyCost, CardKind, CardRarityValue, CardTarget)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await PowerCmd.Apply<RegenPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars[nameof(RegenPower)].BaseValue,
            Owner.Creature,
            this);
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars["MaxHp"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2m);
        DynamicVars[nameof(RegenPower)].UpgradeValueBy(1m);
        DynamicVars["MaxHp"].UpgradeValueBy(1);
    }
}
