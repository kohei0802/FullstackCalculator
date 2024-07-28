using System.Text;
using System.Text.Json;

namespace ClassLibrary1
{
    public class SwaggerRestApi : IDataSource
    {
        string basePath = "http://localhost:5244/";
        public async Task DropAsync()
        {
            HttpClient httpClient = new HttpClient();
            SwaggerClient client = new SwaggerClient(basePath, httpClient);
            await client.DeleteAsync();
        }

        public async Task InsertAsync(MathExpression expression)
        {
            string deserialized = JsonSerializer.Serialize(expression);
            HttpClient httpClient = new HttpClient();

            SwaggerClient client = new SwaggerClient(basePath, httpClient);
            //IWeatherForecastApi client = new WeatherForecastApi();

            var content = new StringContent(deserialized, Encoding.UTF8, "application/json");

            try
            {
                await client.InsertAsync(expression);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error sending POST request: {e.Message}");
            }
        }


        public async Task<List<MathExpression>> ReadAsync()
        {

            List<MathExpression>? list = null;

            HttpClient httpClient = new HttpClient();

            SwaggerClient client = new SwaggerClient(basePath, httpClient);

            string str = "Ricky";
            var newstr = str.SayHi();
            newstr = StringExtensions.SayHi(str);

            var responseBody = await client.ReadAsync();
            return list = responseBody.ToList();
        }
    }
    public static class StringExtensions
    {
        public static string SayHi(this string str) => str + "SayHi";
    }
}
