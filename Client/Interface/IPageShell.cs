using Client.Utilits;
// Используешь PageShallControl? - Реализуй в VM!

namespace Client.Interface
{
    interface IPageShell
    {
        string TitleTop { get; set; }
        RelayCommand GoBackCommand { get; }
    }
}
