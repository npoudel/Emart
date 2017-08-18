using Syncfusion.PMML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Emart.Controllers
{
    public class PredictionController : Controller
    {
        public ActionResult Analysis()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Analysis(string city, string title, string familysize)
        {
            string pmmlFilePath = Server.MapPath("~/App_Data/train.pmml");
            PMMLEvaluator pmmlEvaluator = new PMMLEvaluatorFactory().GetPMMLEvaluatorInstance(pmmlFilePath);
            var anonymousType = new
            {
                city = city,
                title = title,
                familysize = familysize
            };
            PredictedResult predictedResult = pmmlEvaluator.GetResult(anonymousType, null);
            var result = predictedResult.PredictedValue;

            ViewBag.value = result;
            if (ViewBag.value == "0")
            {
                ViewBag.result = "He will not buy";
            }
            else
            {
                ViewBag.result = "He will buy";
            }
            return Content(ViewBag.result);
        }
    }
}