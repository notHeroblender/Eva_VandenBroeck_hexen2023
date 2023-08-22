    public enum CardType
    {
        Move,
        ShockWave,
        Slash,
        Shoot,
        Blitz
    }
    public interface ICard
    {
        CardType Type { get; }

        //Player Player { get; }
    }