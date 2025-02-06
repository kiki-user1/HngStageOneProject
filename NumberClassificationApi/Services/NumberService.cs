using NumberClassificationApi.Models;
using NumberClassificationApi.Interface;

namespace NumberClassificationApi.Services
{
    public class NumberService : INumberService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NumberService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<NumberProperties> ClassifyNumber(double number)
        {
            var properties = new List<string>();

            if (IsArmstrong(number)) properties.Add("armstrong");
            if (number % 2 != 0) properties.Add("odd");
            else properties.Add("even");

            var funFact = await GetFunFact(number);

            return new NumberProperties()
            {
                number = number,
                is_prime = IsPrime(number),
                is_perfect = IsPerfect(number),
                properties = properties,
                digit_sum = DigitSum(number),
                fun_fact = funFact
            };
        }

        private bool IsPrime(double n)
        {
            if (n < 2) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        private bool IsPerfect(double n)
        {
            if (n == 0) return false;
            int sum = 0;
            for (int i = 1; i < n; i++)
            {
                if (n % i == 0) sum += i;
            }
            return sum == n;
        }

        private bool IsArmstrong(double n)
        {
            int digitCount = n.ToString().Length;
            int sum = (int)n.ToString()
                        .Select(d => Math.Pow(d - '0', digitCount))
                        .Sum();
            return sum == n;
        }

        private int DigitSum(double n)
        {
            return Math.Abs(n).ToString().Where(char.IsDigit).Select(d => int.Parse(d.ToString())).Sum();
        }
        private async Task<string?> GetFunFact(double n)
        {
            using var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetStringAsync($"http://numbersapi.com/{n}/math?json");
                var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FunFactResponse>(response);

                return jsonResponse?.Text;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

    }
}
