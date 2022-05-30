namespace WebAdvert.Api.Services;

using System.Threading;
using WebAdvert.Models;

public interface IAdvertStorageService
{
    Task<string> Add(AdvertModel model);

    Task<bool> Confirm(ConfirmAdvertModel model);
}
