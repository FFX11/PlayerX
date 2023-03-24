using PlayerX.ViewModel;

namespace PlayerX.Views;

public partial class FolderVideosPage : ContentPage
{
	public FolderVideosPage()
	{
		InitializeComponent();

        BindingContext = new FolderVideosViewModel();
    }
}