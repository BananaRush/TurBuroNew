using ModelData;
using ModelData.DataCom;
using ModelData.Utilits;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Client.Utilits
{
    class FileLoader
    {
        private DispatcherTimer _loaderTime = new DispatcherTimer();
        private bool _IsLoader = true;
        private string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Config.GetFileDirectory());

        public event Action<double> ProgressMax = delegate { };
        public event Action<double> ProgressValue = delegate { };

        public FileLoader()
        {
            _loaderTime.Tick += TimerUpDate_Tick;
            _loaderTime.Interval = new TimeSpan(0, 0, 5, 0, 0);
        }

        private async void TimerUpDate_Tick(object sender, EventArgs e)
        {
            if (_IsLoader)
            {
                _IsLoader = false;
                await Task.Run(() => {
                    try
                    {
                        Loading();
                    }
                    catch { }
                });
            }
        }

        public void Start()
        {
            _IsLoader = true;
            _loaderTime.Start();
            TimerUpDate_Tick(null, null);
        }

        public void Stop()
        {
            _IsLoader = false;
            _loaderTime.Stop();
        }

        private async void Loading()
        {
            // Запрашиваем данные с сервера
            List<FileModel> listServer = await WebApi.Files.GetAllFiles();
            List<FileModel> listClient = new List<FileModel>();
            FileInfo file = null;
            List<string> filesClient = null;
            List<FileModel> fileLoaded = null;

            if (listServer == null)
            {
                return;
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Получаем файлы клиента
            filesClient = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories).ToList();

            foreach (var item in filesClient)
            {
                file = new FileInfo(item);
                try
                {
                    listClient.Add(new FileModel
                    {
                        Name = file.Name,
                        Path = item.Substring(path.Length),
                        Size = file.Length
                    });
                }
                catch { }
            }
            // Удоляем файлы которых нет на сервере
            DeleteFile(listClient, listServer);
            // Получем файлы которые нужно загрузить
            fileLoaded = LoadFile(listClient, listServer);
            // Устанавливаем максимальное значение загрузки
            SetProgressBarMax(fileLoaded.Sum(w => w.Size));
            // Загружаем файлы
            for (int i = 0; i < fileLoaded.Count; i++)
            {
                // Загружаем
                bool IsFlag = await LoadFile(new FileLoaderModel
                {
                    FileName = fileLoaded[i].Name,
                    Length = fileLoaded[i].Size
                });

                //if(!IsFlag)
                //{
                //    break;
                //}
            }
            SendStatusProgEvent(0);
            _IsLoader = true;
        }

        private List<FileModel> LoadFile(List<FileModel> client, List<FileModel> server)
        {
            return server.Except(client, new ActorComparer()).ToList();
        }

        public async Task<bool> LoadFile(FileLoaderModel file)
        {
            string oldFileName = file.FileName;
            string Patch = Path.Combine(path, SubName() + "_" + file.FileName);
            int BlockSize = 8192;

            try
            {
                using (FileStream fstream = File.Open(Patch, FileMode.Create))
                {
                    do
                    {
                        file.Block = null;
                        file.BlockSize = BlockSize;
                        //Получаем данные
                        file = WebApi.Files.GetFile(file).Result;
                        //Записываем данные в новый файл
                        fstream.Write(file.Block, 0, file.ByteReadCout);
                        file.Length -= file.ByteReadCout;
                        file.Offset += file.ByteReadCout;
                        SendStatusProgEvent(file.ByteReadCout);
                    } while (file.Length != 0);
                }

                File.Move(Patch, Path.Combine(path, oldFileName));
            }
            catch (Exception r)
            {
                File.Delete(Patch);
                return false;
            }

            return true;
        }

        private string SubName()
        {
            return new Random().Next(1000000, 99999999).ToString();
        }

        private void DeleteFile(List<FileModel> client, List<FileModel> server)
        {
            List<FileModel> deleteList = client.Except(server, new ActorComparer()).ToList();

            foreach (var item in deleteList)
            {
                try
                {
                    File.Delete(Path.Combine(path, item.Name));
                    client.Remove(item);
                }
                catch { }
            }
        }

        public void SendStatusProgEvent(double value)
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    ProgressValue(value);
                }));
            }
            catch
            {

            }
        }

        public void SetProgressBarMax(double value)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                ProgressMax(value);
            }));
        }
    }
}
