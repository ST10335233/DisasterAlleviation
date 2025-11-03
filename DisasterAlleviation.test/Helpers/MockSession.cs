using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DisasterAlleviation.Tests.Helpers
{
    public class MockSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionStorage = new();

        public bool IsAvailable => true;
        public string Id => "mockSessionId";
        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public void Clear() => _sessionStorage.Clear();
        public void Remove(string key) => _sessionStorage.Remove(key);

        public void Set(string key, byte[] value) => _sessionStorage[key] = value;

        public bool TryGetValue(string key, out byte[] value) =>
            _sessionStorage.TryGetValue(key, out value);

        public Task LoadAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task CommitAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}
