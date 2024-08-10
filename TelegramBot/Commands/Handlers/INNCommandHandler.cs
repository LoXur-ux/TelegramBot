using Dadata;
using Dadata.Model;
using System.Text;
using TelegramBot.Services;

namespace TelegramBot.Commands.Handlers
{
    public class INNCommandHandler : ICommandHandler
    {
        public string Command => "/inn";
        public string? Data { get; set; }
        public CommandHistoryService CommandHistoryService { get; set; }

        private readonly ISuggestClientAsync _service;

        public INNCommandHandler(ISuggestClientAsync service, CommandHistoryService commandHistoryService)
        {
            _service = service;
            CommandHistoryService = commandHistoryService;
        }

        public async Task<string> HandleAsync(long chatId)
        {
            if (string.IsNullOrWhiteSpace(Data))
            {
                return "Напишите ИНН компании, информацию о которй ищете." +
                    "Например: /inn 7731457980";
            }

            await SaveCommandInHistory(chatId);

            var inns = Data.Trim().Split(' ');
            var suggests = new List<SuggestResponse<Party>>();

            try
            {
                foreach (var inn in inns)
                {
                    var suggest = await _service.SuggestParty(inn.Trim(), 1);
                    suggests.Add(suggest);
                }
            }
            catch (Exception ex) { }

            return GetInfoByParty(suggests);
        }

        private string GetInfoByParty(List<SuggestResponse<Party>> suggests)
        {
            var result = new StringBuilder();
            foreach (var suggest in suggests)
            {
                var party = suggest.suggestions.ElementAt(0);
                result.Append("ИНН компании: ").Append(party.data.inn).Append(Environment.NewLine);
                result.Append("Название компании: ").Append(party.value).Append(Environment.NewLine);
                result.Append("Адрес компании: ").Append(party.data.address.value);

                result.Append(Environment.NewLine).Append(Environment.NewLine);
            }

            return result.ToString();
        }

        public async Task SaveCommandInHistory(long chatId)
        {
            await CommandHistoryService.SaveCommandAsync(chatId, Command, Data);
        }
    }
}
