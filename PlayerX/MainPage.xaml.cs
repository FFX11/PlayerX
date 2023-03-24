using CommunityToolkit.Maui.Views;

using PlayerX.Models;
using PlayerX.ViewModel;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace PlayerX.Views;

public partial class MainPage : ContentPage
{
    int currSong = 0;
    string folderPath;
    string songLocal;
    readonly string pausePng = "pause.png";
    readonly string playPng = "play.png";

    bool repeat = false;
    bool shuffle = false;

    ObservableCollection<MusicInfo> musicList;
    ObservableCollection<MusicInfo> Mp3Files = new();
    List<MusicInfo> ShuffledMp3Files = new();
    ObservableCollection<MusicInfo> OriginalMp3Files = new();
    Random rand = new ();
    public MainPage()
    {
        currSong = 0;
        InitializeComponent();
         
        BindingContext = new MainViewModel(MediaEle);
        
        if (App.FolderSongsViewModel.CurrSong.Location != null)
        {
            var selectedSong = App.FolderSongsViewModel.CurrSong;

            MediaEle.Source = selectedSong.Location;
            MyTitle.Text = selectedSong.SongName;
        }

        if (App.FolderSongsViewModel.FolderPath != null)
        {
            folderPath = App.FolderSongsViewModel.FolderPath;
        }

        if(App.FolderSongsViewModel.SongsList.Count > 0)
        {
            Mp3Files = new();
            currSong = 0;

            var files = App.FolderSongsViewModel.SongsList;

            foreach (var file in files)
            {
                Mp3Files.Add(file);
                OriginalMp3Files.Add(file);
            }
        }

        if (folderPath != null)
        {
            Mp3Files = new();

            foreach (string musicFilePath in Directory.EnumerateFiles(folderPath, "*.mp3", SearchOption.AllDirectories))
            {
                var songFile = new MusicInfo();

                var songName = Path.GetFileName(musicFilePath);

                songFile.Location = musicFilePath;
                songFile.SongName = songName;
                Mp3Files.Add(songFile);
            }
        }
    }

    //private void ContentPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (MediaEle != null)
    //    {
    //        if (MediaEle.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
    //        {
    //            MediaEle.Stop();
    //        }
    //    }
    //}

    private void MediaEle_MediaEnded(object sender, EventArgs e)
    {
        if (Mp3Files.Count > currSong)
        {
            if (shuffle && !repeat)
            {
                MediaEle.Source = null;
                var file = ShuffledMp3Files[rand.Next(0,Mp3Files.Count)];
                MediaEle.Source = file.Location;
                MyTitle.Text = file.SongName;
                MediaEle.Play();
            }
            else if (shuffle && repeat)
            {
                songLocal = MediaEle.Source.ToString();

                var file= songLocal.Substring(5);

                MediaEle.Source = file;
                
                MediaEle.Play();
                
            }
            else
            {
                MediaEle.Source = Mp3Files[currSong].Location;
                MyTitle.Text = Mp3Files[currSong].SongName;
                MediaEle.Play();
                currSong++;
            }

            
        }
        if (MediaEle.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Stopped)
        {
            PlayPauseButton.ImageSource = playPng;
        }
        if (MediaEle.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
        {
            PlayPauseButton.ImageSource = pausePng;
        }
    }

    private void ShuffleButton_Clicked(object sender, EventArgs e)
    {
        var shuffled = Mp3Files.OrderBy(x => Guid.NewGuid()).ToList();
        ShuffledMp3Files = shuffled;
        if (shuffle)
        {
            Mp3Files = new();
            Mp3Files = OriginalMp3Files;

            ShuffleButton.Background = Colors.Transparent;

            shuffle = false;
            return;
        }

        ShuffleButton.Background = Colors.Orange;

        Mp3Files = new();

        foreach (var item in shuffled)
        {
            Mp3Files.Add(item);
        }

        shuffle = true;
    }

    private void RepeatButton_Clicked(object sender, EventArgs e)
    {
        if (repeat)
        {
            RepeatButton.Background = Colors.Transparent;
            MediaEle.ShouldLoopPlayback = false;
            repeat = false;

            return;
        }

        RepeatButton.Background = Colors.Orange;
        MediaEle.ShouldLoopPlayback = true;

        repeat = true;
    }

    private void StopButton_Clicked(object sender, EventArgs e)
    {
        if (MediaEle.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
        {
            MediaEle.Stop();
            MyTitle.Text = string.Empty;
            PlayPauseButton.ImageSource = "play.png";
        }
    }

    private void PlayPauseButton_Clicked(object sender, EventArgs e)
    {
        if (MediaEle.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Paused)
        {
            PlayPauseButton.ImageSource = "pause.png";
            MediaEle.Play();
        } 
        else if(MediaEle.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
        {
            PlayPauseButton.ImageSource = "play.png";
            MediaEle.Pause();
        }
    }

    private void NextButton_Clicked(object sender, EventArgs e)
    {
        if (Mp3Files.Count > 0)
        {
            if (Mp3Files.Count -1 > currSong )
            {
                currSong++;
                MediaEle.Stop();
                MediaEle.Source = Mp3Files[currSong].Location;
                MyTitle.Text = Mp3Files[currSong].SongName;
                PlayPauseButton.ImageSource = "pause.png";
                MediaEle.Play();
            }
        }
    }

    private void PreviousButton_Clicked(object sender, EventArgs e)
    {
        if (Mp3Files.Count > 0)
        {
            if (currSong > 0)
            {
                currSong--;
                MediaEle.Stop();
                MediaEle.Source = Mp3Files[currSong].Location;
                MyTitle.Text = Mp3Files[currSong].SongName;
                PlayPauseButton.ImageSource = "pause.png";
                MediaEle.Play();
            }
        }
    }

    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        var newValue = ((Slider)sender).Value;
        MediaEle.SeekTo(TimeSpan.FromSeconds(newValue));
    }

    private void ContentPage_Disappearing(object sender, EventArgs e)
    {
        if (MediaEle != null)
        {
            if (MediaEle.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
            {
                MediaEle.Stop();
            }
        }
    }
}

