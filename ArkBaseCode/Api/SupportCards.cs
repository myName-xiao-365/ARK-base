using ArkBase.Cards;
using ArkBase.Characters;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace ArkBase.Api;

public static class SupportCards
{
    public static IReadOnlyList<CardModel> GetCards(CardRarity? rarity = null) =>
        ModelDb.AllCards
            .Where(card => card.Pool is ArkSupportCardPool &&
                           (rarity is null || card.Rarity == rarity))
            .DistinctBy(card => card.Id)
            .ToArray();

    public static bool CanPromote(CardModel card) => card is
        SelfRepair or SilentNurture or RongHeJuYing or Polu or MeiYingMiJi or ChixiaoBengye or PureForce or NightEcho or FinalCalamity or RockslideHammer or HeartLash or DimmedAfterglow or HomecomingInvitation;

    public static async Task<CardPileAddResult?> Promote(
        CardModel card,
        CardPreviewStyle previewStyle = CardPreviewStyle.None)
    {
        bool wasUpgraded = card.IsUpgraded;
        CardPileAddResult? result = card switch
        {
            SelfRepair => await CardCmd.TransformTo<StrategyMelt>(card, previewStyle),
            SilentNurture => await CardCmd.TransformTo<Yinyun>(card, previewStyle),
            RongHeJuYing => await CardCmd.TransformTo<Ember>(card, previewStyle),
            Polu => await CardCmd.TransformTo<CandleShadow>(card, previewStyle),
            MeiYingMiJi => await CardCmd.TransformTo<Hemoptysis>(card, previewStyle),
            ChixiaoBengye => await CardCmd.TransformTo<ChixiaoTianwei>(card, previewStyle),
            PureForce => await CardCmd.TransformTo<InnateWarrior>(card, previewStyle),
            NightEcho => await CardCmd.TransformTo<EmptyTheater>(card, previewStyle),
            FinalCalamity => await CardCmd.TransformTo<HeadWolf>(card, previewStyle),
            RockslideHammer => await CardCmd.TransformTo<VerdantSoilForBody>(card, previewStyle),
            HeartLash => await CardCmd.TransformTo<Dreadburst>(card, previewStyle),
            DimmedAfterglow => await CardCmd.TransformTo<CrownOfTheDead>(card, previewStyle),
            HomecomingInvitation => await CardCmd.TransformTo<SalvoForcedRemembrance>(card, previewStyle),
            _ => null
        };

        if (wasUpgraded && result is { } transformed && transformed.cardAdded.IsUpgradable)
        {
            CardCmd.Upgrade(transformed.cardAdded, CardPreviewStyle.None);
        }

        return result;
    }
}
