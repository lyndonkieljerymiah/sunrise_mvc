using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;

namespace Sunrise.Client.Helpers.File
{
    public class FileMediaFormatter : FormUrlEncodedMediaTypeFormatter
    {
        //uploads forms with files and data..
        private const string StringMultipartMediaType = "multipart/form-data";

        //for uploading files only...
        private const string StringApplicationMediaType = "application/octet-stream";

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }



    }
}