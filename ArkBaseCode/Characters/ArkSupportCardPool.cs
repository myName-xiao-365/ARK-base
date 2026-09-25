using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace ArkBase.Characters;

[RegisterSharedCardPool]
public sealed class ArkSupportCardPool : TypeListCardPoolModel
{
    private static readonly Material? PoolFrameTintMaterial =
        MaterialUtils.CreateRgbShaderMaterial(0.62f, 0.82f, 0.96f);

    public override string Title => "ArkSupport";
    public override string EnergyColorName => "ArkBase";
    public override string? BigEnergyIconPath => $"{Entry.ResPath}/images/characters/energy.png";
    public override string? TextEnergyIconPath => $"{Entry.ResPath}/images/characters/energy_text.png";
    public override Color DeckEntryCardColor => new(0.62f, 0.82f, 0.96f);
    public override Color EnergyOutlineColor => new(0.08f, 0.18f, 0.24f);
    public override Material? PoolFrameMaterial => PoolFrameTintMaterial;
    public override bool IsColorless => false;
}
