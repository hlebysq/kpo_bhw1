namespace HseBanking.HseBanking.Application.ImportExport;

public interface IDataExporter
{
    string Export(IEnumerable<object> data);
}