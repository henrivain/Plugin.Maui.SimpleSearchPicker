namespace Plugin.Maui.SimpleSearchPicker;
internal class CooldownTimer
{
    public CooldownTimer() { }
    public CooldownTimer(int delay_ms) => Delay_Ms = delay_ms;


    readonly object _lock = new();

    Guid? _entryId;

    public int Delay_Ms { get; init; } = 1000;

    internal async Task<bool> WaitIsCooldownOverAsync()
    {
        Guid guid = Guid.NewGuid();

        lock (_lock)
        {
            _entryId = guid;
        }

        await Task.Delay(Delay_Ms);

        lock (_lock)
        {
            return _entryId == guid;
        }
    }







}
