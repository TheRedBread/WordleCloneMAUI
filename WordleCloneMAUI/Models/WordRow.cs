using CommunityToolkit.Mvvm.ComponentModel;

namespace WordleCloneMAUI.Models;
public partial class WordRow
{
    public WordRow()
    {
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

