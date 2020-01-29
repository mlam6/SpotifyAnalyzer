using System.Data;
using System.IO;
using OfficeOpenXml;

namespace SpotifyAnalyzer.Service
{
    public class VisualizerService
    {
        public void Get(string directoryPath, DataTable dt)
        {
            using (var p = new ExcelPackage())
            {
                var ws = p.Workbook.Worksheets.Add("Sheet1");
                ws.Cells["A1"].LoadFromDataTable(dt, true);
                p.SaveAs(new FileInfo(directoryPath + "Top50Us.xlsx"));
            }
        }
    }
}
