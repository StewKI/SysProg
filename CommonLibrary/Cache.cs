namespace CommonLibrary;

public class Cache
{
    private Cache()
    { }
    
    private static Cache? instance = null;

    public static Cache Instance
    {
        get
        {
            if (instance is null) instance = new Cache();
            return instance;
        }
    }

    private readonly Dictionary<string, List<Movie>> _cache = new();
    private readonly ReaderWriterLockSlim _lock = new();

    public bool TryGet(string query, out List<Movie> movies)
    {
        _lock.EnterReadLock();
        try
        {
            return _cache.TryGetValue(query, out movies!);
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    public void AddOrUpdate(string query, List<Movie> movies)
    {
        _lock.EnterWriteLock();
        try
        {
            _cache[query] = movies;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool Remove(string query)
    {
        _lock.EnterWriteLock();
        try
        {
            return _cache.Remove(query);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _cache.Clear();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }
}
