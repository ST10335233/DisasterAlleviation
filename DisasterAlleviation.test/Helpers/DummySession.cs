using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class DummySession : ISession
{
    Dictionary<string, byte[]> _data = new();

    public string Id => "test";
    public bool IsAvailable => true;
    public IEnumerable<string> Keys => _data.Keys;

    public void Clear() => _data.Clear();
    public Task CommitAsync(CancellationToken token = default) => Task.CompletedTask;
    public Task LoadAsync(CancellationToken token = default) => Task.CompletedTask;

    public void Remove(string key) => _data.Remove(key);
    public void Set(string key, byte[] value) => _data[key] = value;
    public bool TryGetValue(string key, out byte[] value) => _data.TryGetValue(key, out value);
}
