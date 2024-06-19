namespace BeautySaloon.Services.Interfaces;

public interface ITelegramService
{
    Task SendMessageAsync(long chatId, string message);
}