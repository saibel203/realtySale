using System.Text;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Api.Services.IService;
using RealtySale.Shared.Data;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RealtySale.Api.Services.Service;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    private readonly IUnitOfWork _unitOfWork;

    public MailService(IConfiguration configuration, IWebHostEnvironment environment, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _environment = environment;
        _unitOfWork = unitOfWork;
    }

    public async Task SendEmailAsync(EmailBody body)
    {
        var adminSite = _configuration["SendGripOptions:FromEmail"];
        var userDataResult = await _unitOfWork.UserRepository.GetUserDataAsync(body.Username);

        if (!userDataResult.IsSuccess || userDataResult.User is null)
            throw new Exception(userDataResult.Message);

        var userData = userDataResult.User;
        var pathToFile = _environment.WebRootPath + "\\Templates\\MailTemplate.html";

        using var streamReader = new StreamReader(pathToFile);
        var content = new StringBuilder(await streamReader.ReadToEndAsync());
        
        content.Replace("#AdminName#", adminSite);
        content.Replace("#LetterSubject#", body.Subject);
        content.Replace("#LetterContent#", body.Content);
        content.Replace("#ToEmailName#", body.ToEmail);
        content.Replace("#ToMailName#", body.Username);
        content.Replace("#SenderName#", $"{userData.Email} ({body.Username})");
        
        streamReader.Close();

        var apiKey = _configuration["SendGripOptions:ApiKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(adminSite, "RealtySale admin");
        var to = new EmailAddress(body.ToEmail);
        var message = MailHelper.CreateSingleEmail(from, to, body.Subject, body.Content, content.ToString());
        await client.SendEmailAsync(message);
    }
    
    
}
