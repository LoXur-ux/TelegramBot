﻿using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Commands;
using TelegramBot.Handlers;

namespace TelegramBot.Services
{
    public class TelegramBotHostedServices : IHostedService
    {
        private readonly ITelegramBotClient _client;
        private readonly CommandHandlerFactory _commandHandlerFactory;
        private readonly ErrorHandler _errorHandler;

        public TelegramBotHostedServices(ITelegramBotClient client, CommandHandlerFactory commandHandlerFactory, ErrorHandler errorHandler)
        {
            _client = client;
            _commandHandlerFactory = commandHandlerFactory;
            _errorHandler = errorHandler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var reciverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            _client.StartReceiving(HandleUpdateAsync, HandleErrorAsync, reciverOptions, cancellationToken);
            return Task.CompletedTask;
        }

        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not Message message
                || message.Text is not string messageText)
            {
                return;
            }

            messageText = messageText.ToLower();

            var response = await _commandHandlerFactory.HandleCommandAsync(messageText);

            if (response is not null)
            {
                await client.SendTextMessageAsync(message.Chat.Id, response, cancellationToken: cancellationToken);
            }
        }

        private async Task<string?> HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            return await _errorHandler.HandleError(exception, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
