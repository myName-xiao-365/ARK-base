using ArkBase.Api;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class QedSluggishEnergyPower : ModPowerTemplate, ISluggishAppliedListener
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/characters/energy.png",
        BigIconPath: $"{Entry.ResPath}/images/characters/energy.png");

    public async Task OnSluggishApplied(
        PlayerChoiceContext choiceContext,
        int stacks,
        Creature applier,
        CardModel? cardSource)
    {
        if (stacks > 0 && applier.Player is { } player)
        {
            await PlayerCmd.GainEnergy(stacks, player);
        }
    }
}
