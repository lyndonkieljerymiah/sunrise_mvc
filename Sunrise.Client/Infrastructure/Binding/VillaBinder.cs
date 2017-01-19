using Sunrise.Client.Domains.ViewModels;
using System;
using System.IO;
using System.Web;
using System.Web.Http.ModelBinding;
using Utilities.Enum;
using System.Web.Http.Controllers;
using System.Web.Http;

namespace Sunrise.Client.Infrastructure.Binding
{
    public class VillaBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(VillaViewModel))
            {
                return false;
            }

            var httpRequest = HttpContext.Current.Request;
            var form = httpRequest.Form;
            


            VillaViewModel vm = new VillaViewModel
            {
                Id = form.Get("id"),
                VillaNo = form.Get("villaNo"),
                ElecNo = form.Get("elecNo"),
                WaterNo = form.Get("waterNo"),
                QtelNo = form.Get("qtelNo"),
                Capacity = Convert.ToInt32(form.Get("capacity")),
                Description = form.Get("description"),
                Type = form.Get("type"),
                RatePerMonth = Convert.ToDecimal(form.Get("ratePerMonth"))
            };

            //get gallery for deletion if any
            var keys = string.IsNullOrEmpty(form.Get("forDeletion")) ? 
                "" : 
                form.Get("forDeletion").Substring(0, form.Get("forDeletion").Length - 1);

            if (keys.Length > 0)
            {
                //convert to array
                var arrayKeys = keys.Split(',');
                foreach (var key in arrayKeys)
                {
                    ImageGalleryViewModel gallery = new ImageGalleryViewModel
                    {
                        Id = Convert.ToInt32(key),
                        MarkDeleted = true
                    };
                    vm.ImageGalleries.Add(gallery);
                }
            }

            //upload file transfer to
            //image blob view model
            foreach (string file in httpRequest.Files)
            {
                HttpPostedFile postedFile = httpRequest.Files[file];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    string ext = postedFile.FileName.Substring(postedFile.FileName.IndexOf(".") + 1);
                    using (var reader = new BinaryReader(postedFile.InputStream))
                    {
                        ImageGalleryViewModel gallery = new ImageGalleryViewModel
                        {
                            Blob = new ImageBlob
                            {
                                Size = postedFile.ContentLength,
                                MimeType = postedFile.ContentType,
                                FileFormat = ext,
                                FileName = postedFile.FileName,
                                Blob = reader.ReadBytes(postedFile.ContentLength)
                            },
                            MarkDeleted = false
                        };
                        vm.ImageGalleries.Add(gallery);
                    }
                }
            }

            bindingContext.Model = vm;
            return true;
        }
        
    }

    public class VillaModelBinderProvider : ModelBinderProvider
    {
        private Type _type;
        private VillaBinder _villaBinder;

        public VillaModelBinderProvider(System.Type type, VillaBinder villaBinder)
        {
            _type = type;
            _villaBinder = villaBinder;

        }

        public override IModelBinder GetBinder(HttpConfiguration configuration, Type modelType)
        {
            if(modelType == _type)
            {
                return _villaBinder;
            }
            return null;
        }
    }
}
