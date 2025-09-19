using System.Text;

namespace Zad3;

public static class WebFormatter
{
    public static string FormatMessagePage(string message)
    {
        return FormatPage($"<h1>{message}</h1>");
    }

    public static string FormatRestaurantsPage(List<Restaurant> restaurants)
    {
        StringBuilder sb = new();
        foreach (var restaurant in restaurants)
        {
            sb.Append(FormatRestaurant(restaurant));
        }

        if (restaurants.Count == 0)
        {
            sb.Append("<h1>Nije nadjen nijedan restoran</h1>");
        }

        return FormatPage(sb.ToString());
    }

    public static string FormatRestaurant(Restaurant restaurant)
    {
        var address = string.Join(", ", restaurant.Location.DisplayAddress);

        return $"""
                <div class="restaurant-card">
                    <img src="{restaurant.ImageUrl}" alt="{restaurant.Name}" class="restaurant-image"/>
                    <div class="restaurant-info">
                        <h2 class="restaurant-title">{restaurant.Name}</h2>
                        <p><strong>Ocena:</strong> {restaurant.Rating} ⭐ ({restaurant.ReviewCount} recenzija)</p>
                        <p><strong>Cena:</strong> {restaurant.Price}</p>
                        <p><strong>Adresa:</strong> {address}</p>
                        <p><strong>Telefon:</strong> {restaurant.DisplayPhone}</p>
                        <p><strong>Udaljenost:</strong> {restaurant.Distance:F0} m</p>
                        <a href="{restaurant.Url}" target="_blank" class="view-button">Pogledaj</a>
                    </div>
                </div>
                """;
    }

    public static string FormatPage(string body)
    {
        return $"""
               <!DOCTYPE html>
               <html>
               <head>
                   <title>Restaurant Search</title>
                   <style>{GetCss()}</style>
               </head>
               <body>
                   {body}
               </body>
               </html>
               """;
    }

    private static string GetCss()
    {
        return """
               body {
                   font-family: Arial, sans-serif;
                   background-color: #f4f4f9;
                   margin: 0;
                   padding: 20px;
               }
               
               h1 {
                   text-align: center;
               }
               
               .restaurant-card {
                   display: flex;
                   align-items: flex-start;
                   background: white;
                   border-radius: 10px;
                   padding: 15px 20px;
                   margin: 15px auto;
                   max-width: 600px;
                   box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                   transition: transform 0.2s ease, box-shadow 0.2s ease;
               }
               
               .restaurant-card:hover {
                   transform: translateY(-5px);
                   box-shadow: 0 8px 20px rgba(0,0,0,0.15);
               }
               
               .restaurant-image {
                   width: 120px;
                   height: 120px;
                   object-fit: cover;
                   border-radius: 8px;
                   margin-right: 15px;
                   flex-shrink: 0;
               }

               .restaurant-info {
                   flex: 1;
               }
               
               .restaurant-title {
                   font-size: 1.3rem;
                   margin: 0 0 10px;
                   color: #333;
               }
               
               p {
                   margin: 5px 0;
                   color: #555;
               }
               
               strong {
                   color: #222;
               }

               .view-button {
                   display: inline-block;
                   margin-top: 10px;
                   padding: 8px 16px;
                   background-color: #007bff;
                   color: white;
                   text-decoration: none;
                   border-radius: 6px;
                   transition: background-color 0.2s ease;
               }

               .view-button:hover {
                   background-color: #0056b3;
               }
               """;
    }
}
