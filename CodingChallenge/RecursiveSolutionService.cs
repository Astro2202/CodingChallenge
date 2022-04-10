using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CodingChallenge
{
    public class RecursiveSolutionService : IRecursiveSolutionService
    {
        private int wordLength;
        private int maxCombinationAmount;
        private string inputPath;
        private string[] lines;
        private List<string> words;
        private List<string> parts;

        private readonly IConfiguration _config;
        private readonly ILogger<RecursiveSolutionService> _log;

        public RecursiveSolutionService(IConfiguration config, ILogger<RecursiveSolutionService> log)
        {
            _log = log;
            _config = config;
            words = new List<string>();
            parts = new List<string>();
            wordLength = _config.GetValue<int>("WordFinderSettings:WordLength");
            maxCombinationAmount = _config.GetValue<int>("WordFinderSettings:MaxCombinationAmount");
            inputPath = _config.GetValue<string>("WordFinderSettings:InputPath"); //Input file always copied to output directory
            lines = File.ReadAllLines(inputPath);
        }
        public void Run()
        {
            _log.LogInformation("Running recursive solution:");
            SortLists();
            SearchAndPrintWordsRecursive();
        }
        private void SortLists()
        {
            _log.LogInformation("Sorting input into lists and removing duplicates");
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length == wordLength)
                {
                    bool add = true;
                    for (int j = 0; j < words.Count; j++)
                    {
                        if (words[j] == lines[i])
                        {
                            add = false;
                        }
                    }
                    if (add)
                    {
                        words.Add(lines[i]);
                    }
                }
                else
                {
                    bool add = true;
                    for (int j = 0; j < parts.Count; j++)
                    {
                        if (parts[j] == lines[i])
                        {
                            add = false;
                        }
                    }
                    if (add)
                    {
                        parts.Add(lines[i]);
                    }
                }
            }
            _log.LogInformation("Done");
        }
        //Recursive solution functions for different maximum combinations and different word lengths
        private void SearchAndPrintWordsRecursive()
        {
            _log.LogInformation("Matching characters and searching for words...");
            for (int i = 0; i < words.Count; i++)
            {
                FindPartsCombinationForWord(words[i], words[i], new List<string>(), maxCombinationAmount);
            }
            _log.LogInformation("Recursive match and search algorithm completed");
        }
        private void FindPartsCombinationForWord(string word, string leftOver, List<string> combinedParts, int combinesLeft)
        {
            if (combinesLeft > 0)
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    if (leftOver.StartsWith(parts[i]))
                    {
                        List<string> newCombinedParts = new List<string>(combinedParts);
                        newCombinedParts.Add(parts[i]);
                        FindPartsCombinationForWord(word, leftOver.Substring(parts[i].Length), newCombinedParts, combinesLeft - 1);
                    }
                }
            }
            else
            {
                CheckAndPrintResult(word, combinedParts);
            }
        }
        private void CheckAndPrintResult(string word, List<string> combinedParts)
        {
            string combinedPartsAddedUp = "";
            string result = "";
            for (int i = 0; i < combinedParts.Count; i++)
            {
                combinedPartsAddedUp = combinedPartsAddedUp + combinedParts[i];
                if (i == 0)
                {
                    result = combinedParts[0];
                }
                else
                {
                    result = result + "+" + combinedParts[i];
                }
            }
            if (combinedPartsAddedUp == word)
            {
                result = result + "=" + word;
                Console.WriteLine(result);
            }
        }
    }
}
