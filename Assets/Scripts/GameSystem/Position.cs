public struct Position
{
    private readonly int _q;
    private readonly int _r;
    

    public int Q => _q;
    public int R => _r;
   
    public Position(int q, int r)
    {
        _q = q;
        _r = r;
    }
    public override string ToString()
    {
        return $"Position(Q: {_q}, R: {_r}";
    }
    public static bool operator ==(Position left, Position right)
    {
        return left._q == right._q && left._r == right._r;
    }
    public static bool operator !=(Position left, Position right)
    {
        return !(left == right);
    }
}
