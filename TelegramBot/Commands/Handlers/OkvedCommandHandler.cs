using Dadata;
using Dadata.Model;
using System.Text;
using TelegramBot.Services;

namespace TelegramBot.Commands.Handlers
{
    public class OkvedCommandHandler : ICommandHandler
    {
        public string Command => "/okved";

        public string? Data { get; set; }
        public CommandHistoryService CommandHistoryService { get; set; }

        private readonly ISuggestClientAsync _suggestService;
        private readonly IOutwardClientAsync _outwardService;

        public OkvedCommandHandler(ISuggestClientAsync suggestClientAsync, IOutwardClientAsync outwardClientAsync, CommandHistoryService commandHistoryService)
        {
            _suggestService = suggestClientAsync;
            _outwardService = outwardClientAsync;
            CommandHistoryService = commandHistoryService;
        }

        public async Task<string> HandleAsync(long chatId)
        {
            if (string.IsNullOrWhiteSpace(Data))
            {
                return "Напишите ИНН компании, информацию о которй ищете." +
                    "Например: /okved 7731457980";
            }

            await SaveCommandInHistory(chatId);

            var party = new Party();
            var okvedsString = new List<string>();
            try
            {
                var ttt = await _suggestService.SuggestParty(Data.Trim(), 1);
                party = ttt?.suggestions?.FirstOrDefault()?.data;

                var okvedsCode = GetOkvedCodes(party);
                var okvedsSuggest = new List<SuggestResponse<OkvedRecord>>();
                
                foreach (var code in okvedsCode)
                {
                    var suggestOkved = await _outwardService.Suggest<OkvedRecord>(code, 10);
                    okvedsSuggest.Add(suggestOkved);
                }

                foreach (var okved in okvedsSuggest.Select(x => x.suggestions.First().data).OrderByDescending(x => x.name))
                {
                    okvedsString.Add($"{okved.kod} | {okved.name}");
                }

            }
            catch (Exception ex) { }

            if (!okvedsString.Any())
            {
                return "К сожалению, ничего не было найдено!";
            }

            return GetInfoByParty(okvedsString, party);
        }

        private List<string> GetOkvedCodes(Party party)
        {
            var result = new List<string>() { party.okved};

            // Тут должен быть обработчик массива с ОКВЭД!

            return result;
        }

        private string GetInfoByParty(List<string> okveds, Party party)
        {
            var result = new StringBuilder();
            result.Append($"Для компании {party.name} (ИНН: {party.inn}) была найдена следюущая информация по ОКВЭД:").Append(Environment.NewLine);
            okveds.ForEach(x => result.Append(x).Append(Environment.NewLine));

            return result.ToString();
        }

        public async Task SaveCommandInHistory(long chatId)
        {
            await CommandHistoryService.SaveCommandAsync(chatId, Command, Data);
        }
    }
}
