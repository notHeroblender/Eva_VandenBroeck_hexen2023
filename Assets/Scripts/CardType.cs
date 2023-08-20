    public enum CardType
    {
        Move,
        ShockWave,
        Slash,
        Shoot,
        Rain
    }
    public interface ICard
    {
        CardType Type { get; }

        //Player Player { get; }
    }