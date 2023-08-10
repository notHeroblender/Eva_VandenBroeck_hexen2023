    public enum CardType
    {
        Move,
        ShockWave,
        Slash,
        Shoot,
        Meteor
    }
    public interface ICard
    {
        CardType Type { get; }

        //Player Player { get; }
    }