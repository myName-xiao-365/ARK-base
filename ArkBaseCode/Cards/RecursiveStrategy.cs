using ArkBase.Characters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Cards;

[RegisterCard(typeof(ArkSupportCardPool))]
public sealed class RecursiveStrategy : SupportCardTemplate
{
    public override CardAssetProfile AssetProfile => new(
        PortraitPath: $"{Entry.ResPath}/images/cards/RecursiveStrategy.png");

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("EnergyGain", 1),
        new DynamicVar("DelayedEnergy", 1)
    ];

    public RecursiveStrategy() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PlayerCmd.GainEnergy((int)DynamicVars["EnergyGain"].BaseValue, Owner);
        await PowerCmd.Apply<EnergyNextTurnPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["DelayedEnergy"].BaseValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DelayedEnergy"].UpgradeValueBy(1);
    }
}

[HarmonyPatch(typeof(CardModel), nameof(CardModel.Description), MethodType.Getter)]
internal static class RecursiveStrategyDescriptionPatch
{
    private static void Postfix(CardModel __instance, ref LocString __result)
    {
        if (__instance is RecursiveStrategy recursiveStrategy)
        {
            __result = new LocString(
                "cards",
                recursiveStrategy.IsUpgraded
                    ? "ARK_BASE_CARD_RECURSIVE_STRATEGY.descriptionUpgraded"
                    : "ARK_BASE_CARD_RECURSIVE_STRATEGY.description");
        }
    }
}
