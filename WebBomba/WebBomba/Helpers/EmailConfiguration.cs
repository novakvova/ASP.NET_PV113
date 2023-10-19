namespace WebBomba.Helpers;

public class EmailConfiguration
{
    /// <summary>
    /// Хто відправляє листа
    /// </summary>
    public string From { get; set; } = "super.novakvova@ukr.net";
    /// <summary>
    /// Адреса SMTP сервера
    /// </summary>
    public string SmtpServer { get; set; } = "smtp.ukr.net";
    /// <summary>
    /// Порт на якому працює сервер
    /// </summary>
    public int Port { get; set; } = 2525;
    /// <summary>
    /// Імя користувача для авторизації
    /// </summary>
    public string UserName { get; set; } = "super.novakvova@ukr.net";
    /// <summary>
    /// Пароль, який видав сервер
    /// </summary>
    public string Password { get; set; } = "MVjWbajb9tGqiHgK";
}