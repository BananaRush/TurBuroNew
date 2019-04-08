using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;

namespace StorageAPI.Controllers
{
    public class StatisticController : ApiController
    {
        [HttpGet]
        public async Task<bool> PutStat([FromUri]string gender, [FromUri]string terminalName)
        {
            try
            {

            UserCredential credential;
             string[] Scopes = { SheetsService.Scope.Spreadsheets };
            using (var stream =
                new FileStream(HttpRuntime.AppDomainAppPath+ "/client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = HttpRuntime.AppDomainAppPath;
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential =await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));

                var service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "neuronStats",
                });
                String spreadsheetId = "1mKVcyL0Xo8BHqVbcgTVfvzqM86V7jRUry3V3SKUf0wc";
                String range = terminalName+"!"+((gender=="male")?"B2":"C2");
                SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
                var result = request.Execute();
                int setInt=0;
                if(result.Values.Count!=0)
                 setInt = Int32.Parse(result.Values[0][0].ToString())+1;

                ValueRange valueRange = new ValueRange();
                valueRange.MajorDimension = "COLUMNS";//"ROWS";//COLUMNS

                var oblist = new List<object>() { setInt };
                valueRange.Values = new List<IList<object>> { oblist };
                
                SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                UpdateValuesResponse result2 = update.Execute();
                return true;
            }

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}