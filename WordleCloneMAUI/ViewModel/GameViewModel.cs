using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WordleCloneMAUI.Models;

namespace WordleCloneMAUI.ViewModel;
public partial class GameViewModel : ObservableObject
{

    int rowIndex;
    int columnIndex;

    char[] correctAnwser;

    public char[] KeyboardRow1 { get; }
    public char[] KeyboardRow2 { get; }
    public char[] KeyboardRow3 { get; }

    [ObservableProperty]
    public WordRow[] rows;

    public GameViewModel()
    {
        rows = new WordRow[6]
        {
            new WordRow(),
            new WordRow(),
            new WordRow(),
            new WordRow(),
            new WordRow(),
            new WordRow()
        };

        correctAnwser = "BAGEL".ToCharArray();
        KeyboardRow1 = "QWERTYUIOP".ToCharArray();
        KeyboardRow2 = "ASDFGHJKL".ToCharArray();
        KeyboardRow3 = "<ZXCVBNM>".ToCharArray();
    }


    [RelayCommand]
    public void Enter()
    {
        if (columnIndex != 5) return; // can't enter a word with less than 5 letters
        var correct = Rows[rowIndex].Validate(correctAnwser);

        Console.WriteLine(correct);
        if (correct)
        {
            App.Current.MainPage.DisplayAlertAsync("You Win!", "Congratulations!", "OK");
            return;
        }
        if (rowIndex == 5)
        {
            App.Current.MainPage.DisplayAlertAsync("Game over!", "You are out of turns", "OK");
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
