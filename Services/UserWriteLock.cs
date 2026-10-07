namespace Kt10Validation.Services;

public class UserWriteLock
{
    public object SyncRoot { get; } = new();
}
