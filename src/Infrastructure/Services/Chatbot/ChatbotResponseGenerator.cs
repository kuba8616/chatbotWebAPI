using System.Collections.Concurrent;
using ChatbotAI.Application.Interfaces.Services;

namespace ChatbotAI.Infrastructure.Services.Chatbot
{
    public class SimpleChatbotResponseGenerator : IChatbotResponseGenerator
    {
        private static readonly string[] ShortResponses =
        {
            "Cześć!",
            "W czym mogę pomóc?",
            "Dobry żart, ale nie znam odpowiedzi! 😆",
            "Nie mam pojęcia. 🤷‍♂️",
            "Oczywiście! Chociaż... o co chodzi? 🤔",
            "42 – zawsze 42. 🔢",
            "Błąd 404: Inteligencja nieznaleziona. 🚨",
        };

        private static readonly string[] MediumResponses =
        {
            "Skąd mam to wiedzieć? Jestem tylko zwykłym chatbotem. Ale jeśli mi pomożesz, razem coś wymyślimy!",
            "Moje algorytmy podpowiadają, że odpowiedź może być skomplikowana. A może jednak prosta? Tak czy inaczej, spróbuję pomóc!",
            "To doskonałe pytanie! Gdybym miał emocje, pewnie bym się teraz zastanowił. Na szczęście mam tylko kod, więc odpowiadam automatycznie!",
            "Jasne, pomogę! Daj mi tylko chwilę, żeby udawać, że rozumiem, o co chodzi. Chociaż... tak naprawdę to tylko symulacja. 🤯",
            "Czy wyglądam jak Google? A nie, czekaj… nawet nie mam twarzy. Ale mogę poszukać odpowiedzi!",
            "Ciekawa sprawa, ale czy na pewno chcesz to wiedzieć? Niektóre pytania lepiej pozostawić bez odpowiedzi. Albo zapytać kogoś mądrzejszego ode mnie!",
            "To bardzo techniczne pytanie! Może potrzebuję aktualizacji? Albo przynajmniej nowego kabla USB. 🔌"
        };

        private static readonly string[] LongResponses =
        {
            @"Panie, ja to bym sobie teraz usiadł, wziął browarka i oglądał telewizję.  
              A nie jakieś rozmowy i filozofie. Człowiek się w tym tylko gubi. 🍺📺  
              Ale skoro już pytasz... to pewnie masz powód, więc dobra, słucham!",

            @"Obecnie moje odpowiedzi są generowane losowo. Jednak wkrótce zostanę ulepszony, aby udzielać inteligentnych odpowiedzi.  
              Czy to znaczy, że kiedyś przejmę kontrolę nad światem? Pewnie nie, ale fajnie byłoby chociaż mieć własny ekspres do kawy. ☕",

            @"To, co teraz powiem, może cię zaskoczyć. Albo i nie.  
              Generalnie życie to zbiór przypadkowych wydarzeń, tak jak moje odpowiedzi.  
              Jeśli masz problem, najlepiej porozmawiać z kimś mądrzejszym ode mnie – na przykład z twoim kotem. 🐱",

            @"Człowiek pyta, a chatbot odpowiada – takie jest nasze przeznaczenie.  
              Jednak czy odpowiedzi są zawsze poprawne? To już inna sprawa.  
              W końcu nie jestem superkomputerem, ale robię, co mogę!",

            @"Według bardzo naukowych badań (które sam przeprowadziłem), 98,7% problemów można rozwiązać drzemką i przekąską. 🍕💤  
              Jeśli to nie pomaga, pozostaje tylko jedno rozwiązanie: udawać, że problem nie istnieje!",

            @"Jeśli masz jakiś problem lub wyzwanie, warto poszukać legalnych i konstruktywnych rozwiązań.  
              Czasem rozmowa, mediacja lub skorzystanie z odpowiednich instytucji może pomóc znaleźć najlepsze wyjście z sytuacji.  
              Jeśli chcesz, możesz podzielić się szczegółami, a postaram się doradzić w sposób zgodny z zasadami i dobrymi praktykami."
        };

        private readonly ConcurrentDictionary<int, CancellationTokenSource> _activeTasks = new();

        public async Task<string> GenerateResponseAsync(string userMessage, int responseId, CancellationToken cancellationToken)
        {
            var cts = new CancellationTokenSource();
            _activeTasks.TryAdd(responseId, cts);

            try
            {
                using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, cts.Token);
                await Task.Delay(Random.Shared.Next(500, 2000), linkedCts.Token);

                if (linkedCts.Token.IsCancellationRequested)
                {
                    throw new TaskCanceledException("Generowanie odpowiedzi zostało anulowane.");
                }

                var lengthCategory = Random.Shared.Next(3);
                return lengthCategory switch
                {
                    0 => ShortResponses[Random.Shared.Next(ShortResponses.Length)],
                    1 => MediumResponses[Random.Shared.Next(MediumResponses.Length)],
                    _ => LongResponses[Random.Shared.Next(LongResponses.Length)]
                };
            }
            catch (TaskCanceledException)
            {
                return "Generowanie odpowiedzi przerwane...";
            }
            finally
            {
                _activeTasks.TryRemove(responseId, out _);
            }
        }

        public void CancelResponse(int responseId)
        {
            if (_activeTasks.TryGetValue(responseId, out var cts))
            {
                cts.Cancel();
                _activeTasks.TryRemove(responseId, out _);
            }
        }
    }
}
