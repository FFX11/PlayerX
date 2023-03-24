using PlayerX.Views;

namespace PlayerX;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("playerPage", typeof(MainPage));
        Routing.RegisterRoute("folderSongs", typeof(FolderSongsPage));
    }
}
