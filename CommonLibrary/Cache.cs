using System.Collections.Concurrent;

namespace CommonLibrary;

public class Cache
{
    private Cache() { }

    private static Cache? instance = null;
    public static Cache Instance => instance ??= new Cache();

    private readonly ConcurrentDictionary<string, List<Movie>> _cache = new();

    public bool TryGet(string query, out List<Movie> movies) =>
        _cache.TryGetValue(query, out movies!);

    public void AddOrUpdate(string query, List<Movie> movies) =>
        _cache[query] = movies;

    public bool Remove(string query) =>
        _cache.TryRemove(query, out _);

    public void Clear() =>
        _cache.Clear();
}