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
public sealed class SelfRepair : SupportCardTemplate
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, CardKeyword.Exhaust, ArkKeywords.ReconstructionBody];

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/SelfRepair.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(2m),
        new PowerVar<ReconstructionBodyPower>(1)
    ];

    public SelfRepair() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
        await PowerCmd.Apply<ReconstructionBodyPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars[nameof(ReconstructionBodyPower)].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Heal.UpgradeValueBy(2m);
    }
}
