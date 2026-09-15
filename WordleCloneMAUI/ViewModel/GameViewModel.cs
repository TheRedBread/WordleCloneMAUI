using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WordleCloneMAUI.Models;
using WordleCloneMAUI.Services;

namespace WordleCloneMAUI.ViewModel;
public partial class GameViewModel : ObservableObject
{

    private readonly DictionaryService _dictionaryService;

    int rowIndex;
    int columnIndex;

    char[] correctAnwser;

    public KeyboardLetter[] KeyboardRow1 { get; }
    public KeyboardLetter[] KeyboardRow2 { get; }
    public KeyboardLetter[] KeyboardRow3 { get; }

    [ObservableProperty]
    public WordRow[] rows;

    public GameViewModel(DictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
        rows = new WordRow[6]
        {
            new WordRow(dictionaryService),
            new WordRow(dictionaryService),
            new WordRow(dictionaryService),
            new WordRow(dictionaryService),
            new WordRow(dictionaryService),
            new WordRow(dictionaryService)
        };

        KeyboardRow1 = "QWERTYUIOP"
            .Select(x => new KeyboardLetter { Letter = x })
            .ToArray();

        KeyboardRow2 = "ASDFGHJKL"
            .Select(x => new KeyboardLetter { Letter = x })
            .ToArray();

        KeyboardRow3 = "<ZXCVBNM>"
            .Select(x => new KeyboardLetter { Letter = x })
            .ToArray();    
    }

    public async Task SetRandomWord()
    {
        var word = await _dictionaryService.GetRandomFiveLetterWord();

        correctAnwser = word!.ToUpper().ToCharArray();
    }

    [RelayCommand]
    public async Task Enter()
    {
        if (columnIndex != 5) return; // can't enter a word with less than 5 letters
        bool word_exists = await Rows[rowIndex].CheckIfWordExist();

        Console.WriteLine(word_exists);
        if (!word_exists)
        {
            await App.Current.MainPage.DisplayAlertAsync("incorrect word", "Word doesn't exist!", "OK");
            return;
        }
        var correct = Rows[rowIndex].Validate(correctAnwser);
        UpdateKeyboardColors();
        if (correct)
        {
            await App.Current.MainPage.DisplayAlertAsync("You Win!", "Congratulations!", "OK");
            return;
        }
        else if (rowIndex == 5)
        {
            await App.Current.MainPage.DisplayAlertAsync("Game over!", $"You are out of turns, the correct word was: {new string(correctAnwser)}", "OK");
        }
        else
        {
            rowIndex++;
            columnIndex = 0;
        }
        
    }

    [RelayCommand]
    public void EnterLetter(char letter)
    {
        if(letter == '>')
        {
            Enter();
            return;
        }
        if (letter == '<')
        {
            if (columnIndex == 0) return;
            columnIndex--;
            Rows[rowIndex].Letters[columnIndex].Input = ' ';

            return;
        }

        if(columnIndex == 5) return; // if want to set a 6th letter, returns

        Rows[rowIndex].Letters[columnIndex].Input = letter;
        columnIndex++;
    }

   private void UpdateKeyboardColors()
    {
        var keys = KeyboardRow1
            .Concat(KeyboardRow2)
            .Concat(KeyboardRow3);

        foreach (var key in keys)
        {
            foreach (var row in Rows.Take(rowIndex + 1))
            {
                var letter = row.Letters
                    .FirstOrDefault(x => x.Input == key.Letter);

                if (letter == null)
                    continue;

                if (letter.Color == Colors.Green)
                {
                    key.Color = Colors.Green;
                    break;
                }

                if (letter.Color == Colors.DarkGoldenrod)
                {
                    key.Color = Colors.DarkGoldenrod;
                }
                else if (key.Color != Colors.DarkGoldenrod)
                {
                    key.Color = Colors.Black;
                }
            }
        }
    }
}
public partial class KeyboardLetter : ObservableObject
{
    public char Letter { get; set; }

    [ObservableProperty]
    private Color color = Colors.Gray;
}