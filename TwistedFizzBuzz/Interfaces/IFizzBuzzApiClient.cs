namespace TwistedFizzBuzz.Interfaces
{
    public interface IFizzBuzzApiClient
    {
        Task<Dictionary<int, string>> GetFizzBuzzRuleFromApiAsync();
    }
}
