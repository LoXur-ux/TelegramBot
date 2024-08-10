using Dadata;
using Dadata.Model;
using System.Text;

namespace TelegramBot.Commands
{
    public class INNCommandHandler : ICommandHandler
    {
        public string Command => "/inn";

        private readonly ISuggestClientAsync _service;

        public INNCommandHandler(ISuggestClientAsync service)
        {
            _service = service;
        }

        public async Task<string> HandleAsync(string data = "")
        {
            if (string.IsNullOrWhiteSpace(data)) 
            {
                return "Напишите ИНН компании, информацию о которй ищете." +
                    "Например: /inn 7731457980";
            }

            var inns = data.Trim().Split(' ');
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
    }
}
