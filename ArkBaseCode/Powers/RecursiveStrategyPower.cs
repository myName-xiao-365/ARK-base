using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class RecursiveStrategyPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/characters/energy.png",
        BigIconPath: $"{Entry.ResPath}/images/characters/energy.png");

    // Keep the legacy power compatible with the native next-turn energy timing.
    public override async Task AfterEnergyReset(Player player)
    {
        if (!ReferenceEquals(Owner, player.Creature))
        {
            return;
        }

        int energy = (int)Amount;
        if (energy > 0)
        {
            await PlayerCmd.GainEnergy(energy, player);
        }
        await PowerCmd.Remove(this);
    }
}
