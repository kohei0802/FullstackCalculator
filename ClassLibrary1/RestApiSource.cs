using System.Text.Json;
using System.Text;

namespace ClassLibrary1
{
    public class RestApiSource : IDataSource
    {

        public void Drop()
        {
            List<MathExpression>? list = null;

            HttpClient client = new HttpClient();
            string responseBody = client.GetStringAsync("http://localhost:5244/WeatherForecast/all/delete/").Result;

        }

        public async Task DropAsync()
        {
            HttpClient client = new HttpClient();
            string responseBody = await client.GetStringAsync("http://localhost:5244/WeatherForecast/all/delete/");

        }

        public void Insert(MathExpression expression)
        {
            string deserialized = JsonSerializer.Serialize(expression);

            HttpClient client = new HttpClient();
            //client.PostAsync("http://localhost:5244/WeatherForecast/insert", new MathExpressionContent(deserialized));

            var content = new StringContent(deserialized, Encoding.UTF8, "application/json");

            try
            {
                var response = client.PostAsync("http://localhost:5244/WeatherForecast/insert", content).Result;
                var responseBody = response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error sending POST request: {e.Message}");
            }
            
        }

        public async Task InsertAsync(MathExpression expression)
        {
            string deserialized = JsonSerializer.Serialize(expression);

            HttpClient client = new HttpClient();

            var content = new StringContent(deserialized, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:5244/WeatherForecast/insert", content);
                var responseBody = response.Content.ReadAsStringAsync().Result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error sending POST request: {e.Message}");
            }
        }

        public List<MathExpression> Read()
        {
            List<MathExpression>? list = null;

            HttpClient client = new HttpClient();
            string responseBody = client.GetStringAsync("http://localhost:5244/WeatherForecast/all/read/").Result;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            list = JsonSerializer.Deserialize<List<MathExpression>>(responseBody, options);

            if (list is List<MathExpression> listToReturn)
            {
                return listToReturn;
            }

            return new List<MathExpression>();
        }

        public async Task<List<MathExpression>> ReadAsync()
        {
            List<MathExpression>? list = null;

            HttpClient client = new HttpClient();
            var responseBody = await client.GetStringAsync("http://localhost:5244/WeatherForecast/all/read/");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            list = JsonSerializer.Deserialize<List<MathExpression>>(responseBody, options);

            if (list is List<MathExpression> listToReturn)
            {
                return listToReturn;
            }

            return new List<MathExpression>();
        }
    }
}
