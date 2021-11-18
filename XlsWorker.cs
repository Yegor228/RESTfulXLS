using ClosedXML.Excel;
using RESTfulXLS.Models;
using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace RESTfulXLS;
public class XlsWorker
{
    public string Path;
    public XlsWorker(string path)
    {
        this.Path = path;
    }

    public List<Tender> XlsParser()
    {
        List<Tender> tenders = new List<Tender>();
        var tmp = new string[4];
        using (XLWorkbook wB = new XLWorkbook(Path, XLEventTracking.Disabled))
            foreach (IXLWorksheet wS in wB.Worksheets)
                foreach (IXLRow row in wS.RowsUsed().Skip(1))
                {
                    foreach (IXLColumn clmn in wS.ColumnsUsed())
                    {
                        tmp[Convert.ToInt32(clmn.ColumnNumber()) - 1] = row.Cell(clmn.ColumnNumber()).Value.ToString();
                    }
                    tenders.Add(new Tender
                    {
                        Id = tenders.Count,
                        Name = tmp[0].ToString(),
                        DateStart = DateTime.Parse(tmp[1]),
                        DateEnd = DateTime.Parse(tmp[2]),
                        Url = tmp[3]
                    });
                }
        return tenders;
    }

    public void XlsInput(Tender tender)
    {
        DataTable dt = GetDataTable();

        using (XLWorkbook wB = new XLWorkbook(Path, XLEventTracking.Disabled))
            foreach (IXLWorksheet ws in wB.Worksheets)
            {
                int i = 1;
                while (ws.Cell(i, 1).Value.ToString() != "")
                    i++;
                tender.Id = i;

                dt.Rows.Add(tender.Name, tender.DateStart, tender.DateEnd, tender.Url);
                ws.Cell(i, 1).InsertData(dt);
                wB.SaveAs(Path);
            }
    }

    public string XlsOutput(List<Tender> tenders)
    {
        StringBuilder outputJsonTenders = new StringBuilder();
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        };

        foreach (var a in tenders)
            outputJsonTenders.AppendLine(JsonSerializer.Serialize(a, options));

        return outputJsonTenders.ToString();
    }

    public void XlsDelete(int id)
    {
        using (XLWorkbook wB = new XLWorkbook(Path, XLEventTracking.Disabled))
            foreach (IXLWorksheet ws in wB.Worksheets)
            {
                ws.Row(id).Delete();
                wB.SaveAs(Path);
            }
    }

    public void XlsUpdate(int id, Tender tender)
    {
        id += 2;
        var dt = GetDataTable();
        dt.Rows.Add(tender.Name, tender.DateStart, tender.DateEnd, tender.Url);
        using (XLWorkbook wB = new XLWorkbook(Path, XLEventTracking.Disabled))
        {
            foreach (IXLWorksheet ws in wB.Worksheets)
            {
                if (ws.Cell(id, 1).Value.ToString() != "")
                {
                    ws.Row(id).Delete();
                    ws.Cell(id, 1).InsertData(dt);
                    wB.SaveAs(Path);
                }
                else
                    Console.WriteLine("Incorrect id");
                    
            }    
        }
    }

    public DataTable GetDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("", typeof(string));
        dt.Columns.Add("", typeof(DateTime));
        dt.Columns.Add("", typeof(DateTime));
        dt.Columns.Add("", typeof(string));

        return dt;
    }
}
