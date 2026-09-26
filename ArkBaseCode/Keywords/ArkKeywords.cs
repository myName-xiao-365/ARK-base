using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Content;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Keywords;

namespace ArkBase.Keywords;

[RegisterOwnedCardKeyword(nameof(Sluggish), IconPath = "res://ArkBase/images/powers/SluggishPower.png")]
[RegisterOwnedCardKeyword(nameof(Shiver))]
[RegisterOwnedCardKeyword(nameof(Paralysis))]
[RegisterOwnedCardKeyword(nameof(Silence))]
[RegisterOwnedCardKeyword(nameof(Fear))]
[RegisterOwnedCardKeyword(nameof(Poison))]
[RegisterOwnedCardKeyword(nameof(Regen))]
[RegisterOwnedCardKeyword(nameof(ForcedExit))]
[RegisterOwnedCardKeyword(nameof(ReconstructionBody), IconPath = "res://ArkBase/images/powers/ReconstructionBodyPower.png")]
[RegisterOwnedCardKeyword(nameof(Block), IncludeInCardHoverTip = false)]
[RegisterOwnedCardKeyword(nameof(VerdantSoil), IconPath = "res://ArkBase/images/powers/VerdantSoilPower.png")]
[RegisterOwnedCardKeyword(nameof(Servant), IconPath = "res://ArkBase/images/powers/ServantPower.png")]
[RegisterOwnedCardKeyword(nameof(Wanted), IconPath = "res://ArkBase/images/powers/WantedPower.png")]
[RegisterOwnedCardKeyword(nameof(Marked), IconPath = "res://ArkBase/images/powers/MarkedPower.png")]
public sealed class ArkKeywords
{
    public const string SluggishId = "ARK_BASE_KEYWORD_SLUGGISH";
    public const string ShiverId = "ARK_BASE_KEYWORD_SHIVER";
    public const string ParalysisId = "ARK_BASE_KEYWORD_PARALYSIS";
    public const string SilenceId = "ARK_BASE_KEYWORD_SILENCE";
    public const string FearId = "ARK_BASE_KEYWORD_FEAR";
    public const string PoisonId = "ARK_BASE_KEYWORD_POISON";
    public const string RegenId = "ARK_BASE_KEYWORD_REGEN";
    public const string ForcedExitId = "ARK_BASE_KEYWORD_FORCED_EXIT";
    public const string ReconstructionBodyId = "ARK_BASE_KEYWORD_RECONSTRUCTION_BODY";
    public const string BlockId = "ARK_BASE_KEYWORD_BLOCK";
    public const string VerdantSoilId = "ARK_BASE_KEYWORD_VERDANT_SOIL";
    public const string ServantId = "ARK_BASE_KEYWORD_SERVANT";
    public const string WantedId = "ARK_BASE_KEYWORD_WANTED";
    public const string MarkedId = "ARK_BASE_KEYWORD_MARKED";

    public static readonly CardKeyword Sluggish = Keyword(nameof(Sluggish));
    public static readonly CardKeyword Shiver = Keyword(nameof(Shiver));
    public static readonly CardKeyword Paralysis = Keyword(nameof(Paralysis));
    public static readonly CardKeyword Silence = Keyword(nameof(Silence));
    public static readonly CardKeyword Fear = Keyword(nameof(Fear));
    public static readonly CardKeyword Poison = Keyword(nameof(Poison));
    public static readonly CardKeyword Regen = Keyword(nameof(Regen));
    public static readonly CardKeyword ForcedExit = Keyword(nameof(ForcedExit));
    public static readonly CardKeyword ReconstructionBody = Keyword(nameof(ReconstructionBody));
    public static readonly CardKeyword Block = Keyword(nameof(Block));
    public static readonly CardKeyword VerdantSoil = Keyword(nameof(VerdantSoil));
    public static readonly CardKeyword Servant = Keyword(nameof(Servant));
    public static readonly CardKeyword Wanted = Keyword(nameof(Wanted));
    public static readonly CardKeyword Marked = Keyword(nameof(Marked));

    private static CardKeyword Keyword(string name) =>
        ModContentRegistry.GetQualifiedKeywordId(Entry.ModId, name).GetModCardKeyword();
}
