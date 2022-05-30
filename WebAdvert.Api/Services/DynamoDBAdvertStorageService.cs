using WebAdvert.Models;

namespace WebAdvert.Api.Services;

public class DynamoDBAdvertStorageService : IAdvertStorageService
{
    public DynamoDBAdvertStorageService()
    {
    }

    public Task<string> Add(AdvertModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Confirm(ConfirmAdvertModel model)
    {
        throw new NotImplementedException();
    }
}
