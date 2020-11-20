using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using CoxAuto.Helpers;
using CoxAuto.Models;
using CoxAutomotiveChallenge.IServices;
using CoxAutomotiveChallenge.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoxAutomotiveChallenge.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IImportService _importService;

        public HomeController(IImportService importService)
        {
            _importService = importService;
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult ImportFileData()
        {
            var uploadFile = HttpContext.Request.Form.Files.GetFile("file");
            
            var importSummary = new ImportSummary();
            IList<ImportError> importErrors = new List<ImportError>();

            if (ModelState.IsValid)
            {
                if (uploadFile != null)
                {
                    try
                    {
                        var stream = uploadFile.OpenReadStream();

                        if (!uploadFile.FileName.EndsWith(".csv"))
                        {
                            //Shows error if uploaded file is not csv file
                            ModelState.AddModelError("File", "This file format is not supported");
                            ImportHelpers.AddImportErrorToList(ref importErrors, 0, "Reading CSV", "This file format is not supported");
                            importSummary.ImportErrors = importErrors;
                            return Ok(importSummary);
                        }

                        LogManager.WriteLog("CSV importing started...");
                        //Validate and excecute the complete csv file
                        importSummary = _importService.ImportFileData(stream);
                        LogManager.WriteLog("CSV importing completed...");

                        //Sending result data to View
                        return Ok(importSummary);
                    }
                    catch (Exception e)
                    {
                        LogManager.WriteLog("CSV importing closing with errors...", e);
                        ImportHelpers.AddImportErrorToList(ref importErrors, 0, "Reading CSV", "Could not read the file");
                        importSummary.ImportErrors = importErrors;
                        return Ok(importSummary);
                    }
                }
            }

            ModelState.AddModelError("File", "No file is selected");
            ImportHelpers.AddImportErrorToList(ref importErrors, 0, "Reading CSV", "No file is selected");
            return Ok(importSummary);
        }
    }
}
