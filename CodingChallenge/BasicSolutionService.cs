using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CodingChallenge
{
    public class BasicSolutionService : IBasicSolutionService
    {
        private int wordLength;
        private string inputPath;
        private string[] lines;
        private List<string> words;
        private List<string> parts;

        private readonly IConfiguration _config;
        private readonly ILogger<BasicSolutionService> _log;

        public BasicSolutionService(IConfiguration config, ILogger<BasicSolutionService> log)
        {
            _log = log;
            _config = config;
            words = new List<string>();
            parts = new List<string>();
            wordLength = _config.GetValue<int>("WordFinderSettings:WordLength");
            inputPath = _config.GetValue<string>("WordFinderSettings:InputPath"); //Input file always copied to output directory
            lines = File.ReadAllLines(inputPath);
        }
        public void Run()
        {
            _log.LogInformation("Running basic solution:"); 
            SortLists();
            SearchAndPrintWords();
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

        //Basic solution algortihm for different word lengths
        private void SearchAndPrintWords()
        {
            _log.LogInformation("Matching characters and searching for words...");
            for (int i = 0; i < parts.Count; i++)
            {
                for (int j = i + 1; j < parts.Count; j++)
                {
                    if (parts[i].Length + parts[j].Length == wordLength)
                    {
                        for (int k = 0; k < words.Count; k++)
                        {
                            if (words[k] == parts[i] + parts[j])
                            {
                                Console.WriteLine(parts[i] + "+" + parts[j] + "=" + words[k]);
                            }
                        }
                    }
                }
            }
            _log.LogInformation("Basic match and search algorithm completed");
        }
    }
}