using System.Reactive.Linq;

namespace Zad3;

public static class RestaurantPipeline
{
    public static IObservable<List<Restaurant>> FilterAndSort(this IObservable<IEnumerable<Restaurant>> source)
    {
        return source.Select(restaurants =>
            restaurants
                .Where(r => r.Rating > 4.0)
                .Where(r => r.ReviewCount > 500)
                .OrderByDescending(r => PriceValue(r.Price))
                .ThenByDescending(r => r.ReviewCount)
                .ToList()
        );
    }

    private static int PriceValue(string price)
    {
        return price?.Length ?? 0;
    }
}
