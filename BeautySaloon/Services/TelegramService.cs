using BeautySaloon.Services.Interfaces;
using Telegram.Bot;

namespace BeautySaloon.Services;

public class TelegramService : ITelegramService
{
    private readonly ITelegramBotClient _botClient;

    public TelegramService(string botToken)
    {
        _botClient = new TelegramBotClient(botToken);
    }

    public async Task SendMessageAsync(long chatId, string message)
    {
        await _botClient.SendTextMessageAsync(chatId, message);
    }
}