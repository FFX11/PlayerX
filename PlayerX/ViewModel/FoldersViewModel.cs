using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlayerX.Models;
using PlayerX.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerX.ViewModel;

public partial class FoldersViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<string> folders = new();

    [ObservableProperty]
    ObservableCollection<string> files = new();


    [ObservableProperty]
    ObservableCollection<MusicInfo> songs = new();

    [ObservableProperty]
    List<string> folderSongs = new();

    [ObservableProperty]
    int foldersCount = 0;

    [ObservableProperty]
    string folderName;

    public AsyncRelayCommand<string> SelectedCommand { get; }
    public FoldersViewModel()
    {
        RootDirectory();
        SelectedCommand = new AsyncRelayCommand<string>(Selected);
    }


    MusicInfo selectedFolder;

    public MusicInfo SelectedFolder
    {
        get => selectedFolder;
        set => SetProperty(ref selectedFolder, value);
    }

    async Task Selected(string folder)
    {
        
        SearchSongsInFolder(folder);

        if (folder == null)
        {
            return;
        }
        await Shell.Current.Navigation.PushModalAsync(new FolderSongsPage(folder));
    }

    private void RootDirectory()
    {
        var directories = Directory.GetDirectories("/storage/emulated/0/");

        foreach (var directory in directories.Where(x =>
        !x.Equals("/storage/emulated/0/Android") &&
        !x.Equals("/storage/emulated/0/Alarms") &&
        !x.Equals("/storage/emulated/0/Ringtones") &&
        !x.Equals("/storage/emulated/0/Notifications")))
        {
            foreach (string musicFilePath in Directory.EnumerateFiles(directory, "*.mp3", SearchOption.AllDirectories))
            {
                var d = Directory.GetParent(musicFilePath);

                Folders.Add(d.FullName);
                break;
            }
        }
    }

    private void SearchSongsInFolder(string name)
    {
        Songs = new();

        foreach (string musicFilePath in Directory.EnumerateFiles(name, "*.mp3", SearchOption.AllDirectories))
        {
            MusicInfo selectedFolder = new();

            selectedFolder.SongName = musicFilePath.Substring(musicFilePath.LastIndexOf('/') + 1);
            selectedFolder.Artist = "unknown";
            selectedFolder.Location = musicFilePath;

            Songs.Add(selectedFolder);
        }
    }
}
