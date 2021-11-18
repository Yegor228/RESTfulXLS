using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RESTfulXLS.Models;

namespace RESTfulXLS.Controllers
{
    [Controller]
    public class TenderController : Controller
    {
        public List<Tender> tenders;

        public TenderController()
        {
            var parser = new XlsWorker("Data/Data.xlsx");
            tenders = parser.XlsParser();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TenderView()
        {
            return View(tenders);
        }

        public IActionResult RestfulTenderView()
        {
            return View();
        }


        public string Print()
        {
            var tmp = new XlsWorker("Data/Data.xlsx");
            tenders = tmp.XlsParser();
            var outputTenders = tmp.XlsOutput(tenders);


            return outputTenders;
        }

        [HttpPost]
        public void InsertTender(string name, DateTime dStart, DateTime dEnd, string url)
        {
            var tmp = new XlsWorker("Data/Data.xlsx");
            Tender tnd = new Tender { Name = name, DateStart = dStart, DateEnd = dEnd, Url = url };
            tmp.XlsInput(tnd);
        }

        [HttpPost]
        public void DeleteTender(int id)
        {
            var tmp = new XlsWorker("Data/Data.xlsx");
            tmp.XlsDelete(id);
        }

        [HttpPost]
        public void UpdateTender(int id, string name, DateTime dateStart, DateTime dateEnd, string url)
        {
            Tender tnd = new Tender { Name = name, DateStart = dateStart, DateEnd = dateEnd, Url = url };
            var tmp = new XlsWorker("Data/Data.xlsx");
            tmp.XlsUpdate(id, tnd);
        }

    }
}
