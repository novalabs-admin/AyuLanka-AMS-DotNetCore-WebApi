namespace AyuLanka.AMS.BusinessSevices.Contracts
{
    public interface IGeographyService
    {
        Task<IEnumerable<object>> SearchCitiesAsync(string keyword, int limit = 20);
    }
}
