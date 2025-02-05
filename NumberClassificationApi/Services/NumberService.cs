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
        public async Task<NumberProperties> ClassifyNumber(int number)
        {
            var properties = new List<string>();

            if (IsArmstrong(number)) properties.Add("armstrong");
            if (number % 2 != 0) properties.Add("odd");

            var funFact = await GetFunFact(number);

            if(funFact == null)
            {
                return null;
            }

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

        private bool IsPrime(int n)
        {
            if (n < 2) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        private bool IsPerfect(int n)
        {
            int sum = 0;
            for (int i = 1; i < n; i++)
            {
                if (n % i == 0) sum += i;
            }
            return sum == n;
        }

        private bool IsArmstrong(int n)
        {
            int digitCount = n.ToString().Length;
            int sum = (int)n.ToString()
                        .Select(d => Math.Pow(d - '0', digitCount))
                        .Sum();
            return sum == n;
        }

        private int DigitSum(int n)
        {
            return n.ToString().Select(d => int.Parse(d.ToString())).Sum();
        }

        private async Task<string?> GetFunFact(int n)
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
