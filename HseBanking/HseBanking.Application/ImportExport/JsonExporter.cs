using System.Text.Json;

namespace HseBanking.HseBanking.Application.ImportExport;

public class JsonExporter : IDataExporter
{
    public string Export(IEnumerable<object> data)
    {
        return JsonSerializer.Serialize(data);
    }
}