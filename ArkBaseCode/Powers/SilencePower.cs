using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class SilencePower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/powers/SilencePower.png",
        BigIconPath: $"{Entry.ResPath}/images/powers/SilencePower.png");

    public static bool IsActive(Creature? creature) =>
        creature?.Powers.OfType<SilencePower>().Any(power => power.Amount > 0) == true;

    public static bool IsActing(Creature? creature) =>
        creature?.IsMonster == true && creature.Monster?.IsPerformingMove == true && IsActive(creature);

    public static bool IsActingIn(ICombatState? combatState) =>
        combatState?.Creatures.Any(IsActing) == true;

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner))
        {
            await PowerCmd.Remove(this);
        }
    }
}
