using PlayerX.ViewModel;

namespace PlayerX.Views;

public partial class FoldersPage : ContentPage
{
    public FoldersPage()
	{
		InitializeComponent();

    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        BindingContext = new FoldersViewModel();
    }
}