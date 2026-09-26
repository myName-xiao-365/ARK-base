using Godot;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;
using STS2RitsuLib.Utils;

namespace ArkBase.Characters;

[RegisterSharedCardPool]
public sealed class ArkSupportCardPool : TypeListCardPoolModel
{
    private static readonly Material? PoolFrameTintMaterial =
        MaterialUtils.CreateRgbShaderMaterial(0.74f, 0.62f, 0.88f);

    public override string Title => "ArkSupport";
    // Use native colorless icons without overriding their shared texture mappings.
    public override string EnergyColorName => "colorless";
    public override Color DeckEntryCardColor => new(0.74f, 0.62f, 0.88f);
    public override Color EnergyOutlineColor => new(0.08f, 0.18f, 0.24f);
    public override Material? PoolFrameMaterial => PoolFrameTintMaterial;
    public override bool IsColorless => false;
}
