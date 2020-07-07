using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryOrderScript
{
    public class GoogleSheetHelper
    {
        public string ApplicationName { get; set; }
        private string[] scopes = { SheetsService.Scope.Spreadsheets };
        private GoogleCredential googleCredential;
        private SheetsService sheetService;


        public GoogleSheetHelper(GoogleCredential googleCredential, string applicationName)
        {
            this.googleCredential = googleCredential.CreateScoped(scopes);
            this.ApplicationName = applicationName;
            Init();
        }

        public GoogleSheetHelper(string pathToCredentials, string applicationName)
        {
            var cred = GoogleCredential.FromFile(pathToCredentials);
            cred = cred.CreateScoped(scopes);
            this.googleCredential = cred;
            this.ApplicationName = applicationName;
            Init();
        }

        private void Init()
        {
            sheetService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = ApplicationName,
            }) ;
        }

        public async Task<ValueRange> GetRange(string spreadSheetId, string range)
        {
            var request = sheetService.Spreadsheets.Values.Get(spreadSheetId, range);
            var results = await request.ExecuteAsync();
            return results;
        }

        public async Task Append<T>(IEnumerable<T> models, string spreadSheetId, string range) where T : IToValueList
        {
            ValueRange vr = new ValueRange();
            vr.Values = new List<IList<object>>();
            foreach (var model in models)
            {
                vr.Values.Add(model.ToValueList());
            }

            var request = sheetService.Spreadsheets.Values.Append(vr, spreadSheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            await request.ExecuteAsync();
        }

        public async Task AppendOnlyDistinct<T>(IEnumerable<T> models, string spreadSheetId, string range) where T : IToValueList, IEquatable<T>
        {
            models = models.Distinct();
            await Append(models, spreadSheetId, range);
        }
    }
}
