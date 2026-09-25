using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace ArkBase.Api;

public interface ISluggishDamageOverride
{
    bool IgnoresSluggishDamagePenalty { get; }
}

public interface ISluggishThresholdModifier
{
    int SluggishThresholdReduction { get; }
}

public interface ISluggishAppliedListener
{
    Task OnSluggishApplied(
        PlayerChoiceContext choiceContext,
        int stacks,
        Creature applier,
        CardModel? cardSource);
}

public interface IStunPlayBypass;
