using PlayerX.Models;
using PlayerX.ViewModel;
using System.Collections.ObjectModel;

namespace PlayerX.Views;

public partial class FolderSongsPage : ContentPage
{
    public FolderSongsPage(string folderPath)
    {
        InitializeComponent();

        App.FolderSongsViewModel.FolderPath = folderPath;

        BindingContext = new FolderSongsViewModel();
    }
}