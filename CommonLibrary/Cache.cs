
namespace CommonLibrary;

public class Cache
{
    private Cache() { }

    private static Cache? instance = null;
    public static Cache Instance => instance ??= new Cache();

    private readonly int capacity = 100;

    private readonly Dictionary<string, LinkedListNode<(string Key, List<Movie> Value)>> map = new();
    private readonly LinkedList<(string Key, List<Movie> Value)> lruList = new();
    private readonly object lockObject = new();

    public bool TryGet(string query, out List<Movie> movies)
    {
        lock (lockObject)
        {
            if (map.TryGetValue(query, out var node))
            {
                // Move to front (most recently used)
                lruList.Remove(node);
                lruList.AddFirst(node);

                movies = node.Value.Value;
                return true;
            }
        }

        movies = null!;
        return false;
    }

    public void AddOrUpdate(string query, List<Movie> movies)
    {
        lock (lockObject)
        {
            if (map.TryGetValue(query, out var existingNode))
            {
                // update existing
                lruList.Remove(existingNode);
            }

            var newNode = new LinkedListNode<(string, List<Movie>)>((query, movies));
            lruList.AddFirst(newNode);
            map[query] = newNode;

            if (map.Count > capacity)
            {
                // remove least recently used (last node)
                var lru = lruList.Last!;
                lruList.RemoveLast();
                map.Remove(lru.Value.Key);
            }
        }
    }

    public bool Remove(string query)
    {
        lock (lockObject)
        {
            if (map.TryGetValue(query, out var node))
            {
                lruList.Remove(node);
                map.Remove(query);
                return true;
            }
            return false;
        }
    }

    public void Clear()
    {
        lock (lockObject)
        {
            lruList.Clear();
            map.Clear();
        }
    }
}
