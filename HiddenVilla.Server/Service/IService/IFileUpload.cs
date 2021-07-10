using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla.Server.Service.IService
{
    public interface IFileUpload
    {
        public Task<string> UploadFile(IBrowserFile file);
        public bool DeleteFile(string fileName);

    }
}
