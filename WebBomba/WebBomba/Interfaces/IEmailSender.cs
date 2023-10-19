using WebBomba.Models;

namespace WebBomba.Interfaces;


public interface IEmailSender
{
    Task SendAsync(EmailViewModel message);
}