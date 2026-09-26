using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class ServantPower : ModPowerTemplate
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/powers/ServantPower.png",
        BigIconPath: $"{Entry.ResPath}/images/powers/ServantPower.png");

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (!ReferenceEquals(Owner, player.Creature) || !Owner.IsAlive)
        {
            return;
        }

        if (player.Osty is not { IsAlive: true } osty)
        {
            await PowerCmd.Remove(this);
            return;
        }

        if (osty.CombatState is not { } combatState || !combatState.HittableEnemies.Any())
        {
            return;
        }

        Flash();
        // FromOsty only stores the card source; this automatic attack replaces it with the power.
        var attack = DamageCmd.Attack(osty.CurrentHp)
            .FromOsty(osty, null!)
            .TargetingRandomOpponents(combatState);
        attack.ModelSource = this;
        await attack.Execute(choiceContext);
    }

    public override async Task AfterDeath(
        PlayerChoiceContext choiceContext, Creature creature, bool wasRemovalPrevented, float deathAnimLength)
    {
        // Osty's corpse can remain in combat; removal prevention is not survival.
        if (Owner.Player is { } player && creature.Monster is Osty && creature.IsDead &&
            ReferenceEquals(creature.PetOwner, player))
        {
            await PowerCmd.Remove(this);
        }
    }
}
