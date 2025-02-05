using NumberClassificationApi.Models;

namespace NumberClassificationApi.Interface
{
    public interface INumberService
    {
        Task<NumberProperties> ClassifyNumber(int number);
    }
}
