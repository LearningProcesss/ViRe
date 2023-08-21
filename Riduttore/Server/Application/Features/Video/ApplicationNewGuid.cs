public delegate Guid NewGuid();

public static class ApplicationNewGuid
{
    public static Guid NewGuid()
    {
        return Guid.NewGuid();
    }
}