using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Buy.Models
{
    [NotMapped]
    public class CloudFileViewModel : CloudFile
    {
        public CloudFileViewModel()
        {
        }

        public CloudFileViewModel(CloudFile file)
        {
            CreateDateTime = file.CreateDateTime;
            Extension = file.Extension;
            ID = file.ID;
            Name = file.Name;
            Size = file.Size;
            Type = file.Type;
            Url = file.Url;
            User = file.User;
            UserID = file.UserID;
            _thumbnail = null;
            switch (Type)
            {
                case Enums.FileType.Image:
                    _thumbnail = Comm.ResizeImage(Url, 80, 80, mode: Enums.ResizerMode.Crop, scale: Enums.ReszieScale.Both);
                    break;
                case Enums.FileType.Video:
                    break;
                case Enums.FileType.Text:
                    break;
                case Enums.FileType.Audio:
                    break;
                default:
                    break;
            }
        }

        public string _thumbnail;

        public string Thumbnail
        {
            get
            {
                return _thumbnail;
            }
        }
    }
}