using CommunityToolkit.Mvvm.ComponentModel;
using System.Net;
using WordleCloneMAUI.Services;

namespace WordleCloneMAUI.Models;
public partial class WordRow
{
    private readonly DictionaryService _dictionaryService;

    public WordRow(DictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
        Letters = new Letter[5]
        {
            new Letter(),
            new Letter(),
            new Letter(),
            new Letter(),
            new Letter()
        };
    }

    public Letter[] Letters { get; set; }
    public bool Validate(char[] correctAnwser)
    {
        int count = 0;
        for (int i = 0; i < Letters.Length; i++)
        {
            var letter = Letters[i];
            if(letter.Input == correctAnwser[i])
            {
                letter.Color = Colors.Green;
                count++;
            }
            else if (correctAnwser.Contains(letter.Input))
            {
                letter.Color = Colors.DarkGoldenrod;
            }
            else
            {
                letter.Color = Colors.Gray;
            }
        }

        return count == 5; //if every letter is correct then it's true
    }

    internal async Task<bool> CheckIfWordExist()
    {
        string word = "";
        for (int i = 0; i < Letters.Length; i++)
        {
            word += Letters[i].Input;

        }
        return await _dictionaryService.DoesWordExist(word);
    }
}

public partial class Letter : ObservableObject
{
    public Letter() 
    {
        Color = Colors.Black;
    }

    [ObservableProperty]
    private char input;

    [ObservableProperty]
    public Color color;
}


