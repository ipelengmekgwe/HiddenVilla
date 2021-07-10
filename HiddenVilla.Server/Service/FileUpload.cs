using HiddenVilla.Server.Service.IService;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HiddenVilla.Server.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public FileUpload(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public bool DeleteFile(string fileName)
        {
            try
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "RoomImages", fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            try
            {
                FileInfo fileInfo = new(file.Name);
                var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                var folderDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "RoomImages");
                var path = Path.Combine(folderDirectory, fileName);

                MemoryStream memoryStream = new();
                await file.OpenReadStream().CopyToAsync(memoryStream);

                if (!Directory.Exists(folderDirectory)) Directory.CreateDirectory(folderDirectory);

                await using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                memoryStream.WriteTo(fs);

                string url = string.Empty;
#if DEBUG
                url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                //url = $"{_configuration.GetValue<string>("ServerUrlLocal")}";
#else
                url = $"{_configuration.GetValue<string>("ServerUrl")}";
#endif

                //if (System.Diagnostics.Debugger.IsAttached)
                //{
                //    url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                //}
                //else
                //{
                //    url = $"{_configuration.GetValue<string>("ServerUrl")}";
                //}

                var fullPath = $"{url}RoomImages/{fileName}";
                return fullPath;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
