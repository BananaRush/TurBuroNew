using ModelData;

namespace StorageAPI.Models
{
    public enum NewsContentType
    {
        // Ткстовая инфа
        [Displ("Page")]
        Page,
        // Браузерная инфо
        [Displ("Url")]
        Uri,
        // ФАЙЛЫ
        [Displ("Pdf")]
        Pdf,
        [Displ("Presentation")]
        Presentation,
        [Displ("Image")]
        Image,
        [Displ("Section")]
        Section,
        [Displ("NewLents")]
        NewLents,
        [Displ("Video")]
        Video
    }
}