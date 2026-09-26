using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Scaffolding.Content.Patches;

namespace ArkBase.Cards;

public abstract class SupportCardTemplate(
    int energyCost,
    CardType cardType,
    CardRarity rarity,
    TargetType targetType)
    : ModCardTemplate(energyCost, cardType, rarity, targetType, showInCardLibrary: true)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Retain, CardKeyword.Exhaust];

    // Only the cost badge follows the owner; the Support pool still supplies the frame.
    public override string? CustomEnergyIconPath =>
        IsMutable && Owner is { } owner
            ? (owner.Character.CardPool as IModBigEnergyIconPool)?.BigEnergyIconPath
              ?? owner.Character.CardPool.EnergyIconPath
            : base.CustomEnergyIconPath;

    public override bool CanBeGeneratedInCombat => false;
    public override bool CanBeGeneratedByModifiers => false;
}
