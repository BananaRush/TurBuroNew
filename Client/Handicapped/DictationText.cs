using ModelDate.Utilities;
using SpeechLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using VSHIM.Control.WebControl;

namespace VSHIM.Control.Handicapped
{
    static class DictationText
    {
        private static SpVoice voice = new SpVoice();
        private static string SpeeText = string.Empty;

        static DictationText()
        {
        
        }

        public static void Play(object Element)
        {
            if(IsButton(Element))
            {
                Button elm = (Button)Element;
                SpeeText += "Кнопка ";
                Play(elm.Content);
            }

            if(IsTextBlock(Element))
            {
                TextBlock textBlock = (TextBlock)Element;
                SpeeText += textBlock.Text;
                Sound();
            }

            if (IsGrid(Element))
            {
                Grid grid = (Grid)Element;
                foreach(var item in grid.Children)
                {
                    Play(item);
                }
            }

            if(IsString(Element))
            {
                string text = (string)Element;
                SpeeText += text;
                Sound();
            }

            if(IsWebBrowser(Element))
            {
                ChromiumBrowser brs = Element as ChromiumBrowser;
                string text = StrManipulation.ClearHtml(brs.LoadHtml);
                SpeeText += text;
                Sound();
            }
        }

        private static bool IsWebBrowser(object Element)
        {
            return Element is ChromiumBrowser;
        }

        private static bool IsButton(object Element)
        {
            return Element is Button;
        }

        private static bool IsTextBlock(object Element)
        {
            return Element is TextBlock;
        }

        private static bool IsGrid(object Element)
        {
            return Element is Grid;
        }

        private static bool IsString(object Element)
        {
            return Element is string;
        }

        private static void Sound(string text)
        {
            if(!string.IsNullOrEmpty(text))
            {
                voice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
            }
        }

        private static void Sound()
        {
            if (!string.IsNullOrEmpty(SpeeText))
            {
                voice.Speak(SpeeText, SpeechVoiceSpeakFlags.SVSFlagsAsync);
                SpeeText = string.Empty;
            }
        }

        public static void SpeekCansel()
        {
            voice.Skip("Sentence", int.MaxValue);
        }
    }
}
