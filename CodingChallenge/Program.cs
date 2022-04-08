const int WORDLENGTH = 6;
const int MAXCOMBINATION = 2;
string[] lines = File.ReadAllLines(@"input.txt"); //Input file always copied to output directory
List<String> words = new List<String>();
List<String> parts = new List<String>();

//Devide lines into separate lists of complete words and parts and remove duplicate data
for (int i = 0; i < lines.Length; i++)
{
    if(lines[i].Length == WORDLENGTH)
    {
        bool add = true;
        for(int j = 0; j < words.Count; j++)
        {
            if(words[j] == lines[i])
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

//Basic solution completed within time margin
BasicSolution();
//Recursive solution. Uncomment to test.
//RecursiveSolution();

//Basic solution functions for different word lengths
void BasicSolution()
{
    for (int i = 0; i < parts.Count; i++)
    {
        for (int j = i + 1; j < parts.Count; j++)
        {
            if (parts[i].Length + parts[j].Length == WORDLENGTH)
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
}

//Recursive solution functions for different maximum combinations and different word lengths
void RecursiveSolution()
{
    for (int i = 0; i < words.Count; i++)
    {
        FindPartsCombinationForWord(words[i], words[i], new List<string>(), MAXCOMBINATION);
    }
}

void FindPartsCombinationForWord(string word, string leftOver, List<string> combinedParts, int combinesLeft)
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

void CheckAndPrintResult(string word, List<string> combinedParts)
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

Console.WriteLine("Press any key to exit");
Console.ReadKey();