using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlayerX.Models;
using PlayerX.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerX.ViewModel;


public partial class VideoViewModel : ObservableObject
{
    [ObservableProperty]
    public ObservableCollection<string> folders = new();

    [ObservableProperty]
    string folderPath;

    string folderName;

    public ObservableCollection<MusicInfo> Videos = new();
    public AsyncRelayCommand<string> SelectedCommand { get; }

    public VideoViewModel()
    {
        SelectedCommand = new AsyncRelayCommand<string>(Selected);
        RootDirectory();
    }

    string selectedFolder;

    public string SelectedFolder
    {
        get { return selectedFolder; }
        set { selectedFolder = value; }
    }

    async Task Selected(string folder)
    {
        SearchSongsInFolder(folder);

        if (folder == null)
        {
            return;
        }

        App.VideoModel.FolderPath = folder;

        await Shell.Current.Navigation.PushModalAsync(new FolderVideosPage());
    }

    private void SearchSongsInFolder(string name)
    {
        Videos = new();


        foreach (string musicFilePath in Directory.EnumerateFiles(name, "*.mp4", SearchOption.AllDirectories).Where(x => x.EndsWith(".mp4") || x.EndsWith(".wmv")))
        {
            MusicInfo selectedFolder = new();

            selectedFolder.SongName = musicFilePath.Substring(musicFilePath.LastIndexOf('/') + 1);
            selectedFolder.Artist = "unknown";
            selectedFolder.Location = musicFilePath;

            Videos.Add(selectedFolder);
        }
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

            foreach (string musicFilePath in Directory.EnumerateFiles
                (directory, "*.*", SearchOption.AllDirectories)
                .Where(x => x.EndsWith(".mp4") || x.EndsWith(".wmv")))
            {
                var d = Directory.GetParent(musicFilePath);

                Folders.Add(d.FullName);
                break;
            }
        }
    }
}

