using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace MHRiseTalismansFilter
{
	public partial class Form1
	{
        #region private-field
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Update Google Sheet Data with Google Sheets API v4";
        static string spreadsheetId = "15BPfQuh1WclFa2gpqThkSpiFh848qAxyFdmKOH6tzAk";
        static string sheetName = "總表";
        #endregion private-field

        #region private-method
        private static SheetsService OpenSheet()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                //存儲憑證到credPath
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            //建立一個API服務，設定請求參數
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }

        private void LoadSheet_Click(object sender, EventArgs e)
        {
            DecorationSystem.Instance.Filt();
            var service = OpenSheet();

            //設定讀取A欄最後一行位置
            var sRange = string.Format($"{sheetName}!A:H");
            var getRequest = service.Spreadsheets.Values.Get(spreadsheetId, sRange);
            var rVR = getRequest.Execute(); //到Google sheet讀取內容
            var values = rVR.Values; //最後一行位置

            //寫入新資料
            var index = 1;
            var valueRange = new ValueRange();
            sRange = string.Format($"{sheetName}!I:I");  //指定寫入位置
            foreach (var d in DecorationSystem.Instance.List)
            {
                var oblist = new List<object>();
                valueRange.Values = valueRange.Values ?? new List<IList<object>>();
                if (d.ParentId != -1)
                {
#if DEBUG
                    Console.WriteLine("{0}, {1}", index, d.ParentId);
#endif
                    valueRange.Range = sRange;
                    valueRange.MajorDimension = "COLUMNS";//ROWS 或 COLUMNS
                    oblist.Add(d.ParentId);
                }
                else
                {
                    oblist.Add("");
                }
                valueRange.Values.Add(oblist);

                index++;
            }
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, sRange);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var uUVR = updateRequest.Execute();
        }
        #endregion private-method
	}
}
