using ArkBase.Characters;
using ArkBase.Keywords;
using ArkBase.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class VerdantSoilForBody : SupportCardTemplate
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [.. base.CanonicalKeywords, ArkKeywords.Block, ArkKeywords.VerdantSoil];

    public override bool GainsBlock => true;

    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/VerdantSoilForBody.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(15m, ValueProp.Move),
        new PowerVar<VerdantSoilPower>(1)
    ];

    public VerdantSoilForBody() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        await PowerCmd.Apply<VerdantSoilPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars[nameof(VerdantSoilPower)].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Block.UpgradeValueBy(3m);
    }
}
