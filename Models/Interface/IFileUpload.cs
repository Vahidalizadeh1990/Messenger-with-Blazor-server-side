using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Interface
{
    /// <summary>
    /// IFileUpload interface is used for uploading image to wwwroot folder
    /// We inject this interface in our components
    /// </summary>
    public interface IFileUpload
    {
        Task Upload(IFileListEntry file);
        void Remove(string name);
    }
}
