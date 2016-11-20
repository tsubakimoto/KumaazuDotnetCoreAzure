using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using KumaazuDotnetCoreAzure.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KumaazuDotnetCoreAzure.Controllers
{
    public class BlobsController : Controller
    {
        private readonly StorageOption storage;

        public BlobsController(IOptions<StorageOption> _storage)
        {
            storage = _storage.Value;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var blobs = await Models.Blob.Get(storage);
            return View(blobs);
        }
    }
}
