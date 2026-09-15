using WordleCloneMAUI.ViewModel;

namespace WordleCloneMAUI
{
    public partial class MainPage : ContentPage
    {

        public MainPage(GameViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
