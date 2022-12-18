using RealtySale.Shared.Data;

namespace RealtySale.Api.Services.IService;

public interface IMailService
{
    Task SendEmailAsync(EmailBody body);
}