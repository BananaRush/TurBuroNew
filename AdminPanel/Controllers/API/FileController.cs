using ModelData.DataCom;
using ModelData.Model.Database;
using ModelData.Models.Database;
using StorageAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AdminPanel.Controllers.API
{
    public class FileController : ApiController
    {
        [HttpGet]
        public async Task<List<FileModel>> GetAllFiles()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileLoad");
            FileInfo file = null;
            List<string> files = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.AllDirectories).ToList();
            List<FileModel> rt = null;

            if (files != null)
            {
                rt = new List<FileModel>();
                foreach (var item in files)
                {
                    file = new FileInfo(item);
                    try
                    {
                        rt.Add(new FileModel
                        {
                            Name = file.Name,
                            Path = item.Substring(path.Length),
                            Size = file.Length
                        });
                    }
                    catch { }
                }
            }

            return rt;
        }

        [HttpPost]
        public async Task<FileLoaderModel> GetFileBlock([FromBody]FileLoaderModel value)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileLoad", value.FileName);

                using (FileStream fstream = File.Open(path, FileMode.Open))
                {
                    byte[] array = new byte[value.BlockSize];
                    fstream.Seek(value.Offset, SeekOrigin.Current);
                    value.ByteReadCout = fstream.Read(array, 0, array.Length);
                    value.Block = array;
                    fstream.Close();
                }

                return value;
            }
            catch (FileNotFoundException e)
            {
                return new FileLoaderModel();
            }
            catch (Exception e)
            {
                return new FileLoaderModel();
            }
        }

        [HttpPost]
        public bool SaveFile(FileLoaderModel value)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, value.FileName);

            try
            {
                using (FileStream fstream = File.Open(path, FileMode.Append))
                {
                    fstream.Seek(value.Offset, SeekOrigin.Current);
                    fstream.Write(value.Block, 0, value.BlockSize);
                    fstream.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }

        [HttpGet]
        public List<string> GetVideo()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Video");

            if (Directory.Exists(path))
            {
                return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).Where(str => str.EndsWith(".png")
                        || str.EndsWith(".avi")
                        || str.EndsWith(".mkv")
                        || str.EndsWith(".mov")
                        || str.EndsWith(".wma")
                        || str.EndsWith(".mp4")

                        || str.EndsWith(".AVI")
                        || str.EndsWith(".MKV")
                        || str.EndsWith(".MOV")
                        || str.EndsWith(".WMA")
                        || str.EndsWith(".MP4")).ToList();
            }

            return null;
        }
    }
}