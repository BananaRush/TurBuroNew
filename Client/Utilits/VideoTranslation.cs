using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Controls;

namespace Client.Utilits
{
    public enum VideoStatus : int { Non = 0, NonCam, Expect, On, Failed }

    public class VideoTranslation
    {
        private VideoCaptureDevice _videoCaptureDevice;
        private byte[] _bufImage = null;

        public delegate void ByteImage(byte[] img);
        public event ByteImage NewFrame = delegate { };

        public delegate void BitImage(System.Drawing.Bitmap img);
        public event BitImage BitFrame;

        public VideoTranslation()
        {
            Init();
        }

        public void Init()
        {
            try
            {
                var VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (VideoCaptureDevices.Count > 0)
                {
                    _videoCaptureDevice = new VideoCaptureDevice(VideoCaptureDevices[0].MonikerString);
                    _videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                    Status = VideoStatus.Expect;
                }
                else
                {
                    Status = VideoStatus.NonCam;
                }
            }
            catch (Exception ex)
            {
                Status = VideoStatus.Failed;
            }
        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            System.Drawing.Image img = eventArgs.Frame;
            BitFrame?.Invoke(eventArgs.Frame);
            using (var ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                _bufImage = ms.ToArray();
                // Вызываем события
                NewFrame(_bufImage);
            }
        }

        public void Start()
        {
            try
            {
                if (Status == VideoStatus.Expect && !_videoCaptureDevice.IsRunning)
                {
                    _videoCaptureDevice.Start();
                    Status = VideoStatus.On;
                }
            }
            catch (Exception ex)
            {
                Status = VideoStatus.Failed;
            }
        }

        public void Stop()
        {
            try
            {
                if (Status == VideoStatus.On && _videoCaptureDevice.IsRunning)
                {
                    _videoCaptureDevice.Stop();
                    Status = VideoStatus.Expect;
                }
            }
            catch (Exception ex)
            {
                Status = VideoStatus.Failed;
            }
        }

        public byte[] BufferImage
        {
            get => _bufImage;
        }

        public VideoStatus Status { get; set; } = VideoStatus.Non;
    }
}
