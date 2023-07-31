    public enum CardType
    {
        Move,
        ShockWave,
        Slash,
        Shoot
    }
    public interface ICard
    {
        CardType Type { get; }

        //Player Player { get; }
    }