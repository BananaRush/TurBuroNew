using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSHIM.Control.Handicapped.Utilits
{
    public class AudioMng
    {
        public WaveIn input;
        public bool IsStart = false;

        public AudioMng()
        {
            input = new WaveIn();
            input.WaveFormat = new WaveFormat(8000, 16, 1);
            //input.DataAvailable += Input_DataAvailable;
        }

        public void Start()
        {
            IsStart = true;
            input.StartRecording();
        }

        public void Stop()
        {
            IsStart = false; 
            input.StopRecording();
        }

        private void Input_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            byte[] buff = e.Buffer;
            // Отправляем buff в соккет.
        }
    }
}
