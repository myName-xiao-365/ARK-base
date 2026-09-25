using ArkBase.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace ArkBase.Powers;

[RegisterPower]
public sealed class HeadWolfCooldownPower : ModPowerTemplate
{
    private readonly Dictionary<HeadWolf, int> _remainingAttacks =
        new(ReferenceEqualityComparer.Instance);

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerAssetProfile AssetProfile => new(
        IconPath: $"{Entry.ResPath}/images/powers/{GetType().Name}.png",
        BigIconPath: $"{Entry.ResPath}/images/powers/{GetType().Name}.png");

    public void Track(HeadWolf card)
    {
        _remainingAttacks[card] = HeadWolf.CooldownAttacks;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.Type != CardType.Attack ||
            cardPlay.Card.Owner != Owner.Player ||
            _remainingAttacks.Count == 0)
        {
            return;
        }

        List<HeadWolf> ready = [];
        foreach ((HeadWolf card, int remaining) in _remainingAttacks.ToArray())
        {
            if (card.Pile?.Type != PileType.Exhaust)
            {
                _remainingAttacks.Remove(card);
                continue;
            }

            if (ReferenceEquals(cardPlay.Card, card))
            {
                continue;
            }

            if (remaining == 1)
            {
                ready.Add(card);
            }
            else
            {
                _remainingAttacks[card] = remaining - 1;
            }
        }

        foreach (HeadWolf card in ready)
        {
            _remainingAttacks.Remove(card);
            await CardPileCmd.Add(card, PileType.Hand, CardPilePosition.Top, this, false);
        }

        if (_remainingAttacks.Count == 0)
        {
            await PowerCmd.Remove(this);
            return;
        }

        int visibleCount = _remainingAttacks.Values.Min();
        if (visibleCount != Amount)
        {
            await PowerCmd.ModifyAmount(choiceContext, this, visibleCount - Amount, Owner, null);
        }

        Flash();
    }
}
