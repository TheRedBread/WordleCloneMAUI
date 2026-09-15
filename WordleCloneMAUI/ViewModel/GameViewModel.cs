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

    public char[] KeyboardRow1 { get; }
    public char[] KeyboardRow2 { get; }
    public char[] KeyboardRow3 { get; }

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

        KeyboardRow1 = "QWERTYUIOP".ToCharArray();
        KeyboardRow2 = "ASDFGHJKL".ToCharArray();
        KeyboardRow3 = "<ZXCVBNM>".ToCharArray();
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


}
