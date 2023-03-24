using PlayerX.ViewModel;
using PlayerX.Views;

namespace PlayerX;

public partial class App : Application
{
    //public static MainViewModel MainViewModel { get; private set; }
    //public static SongsViewModel SongViewModel { get; private set; }
    public static MainViewModel Vm { get; private set; }
    //public static MainPage MainPageUi { get; private set; }
    public static FolderSongsViewModel FolderSongsViewModel { get;  set; }
    public static VideoViewModel VideoModel { get; set; }
    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
    }
}
