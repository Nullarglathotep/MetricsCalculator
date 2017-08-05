namespace MetricsCalculator
{
    public interface IDataStoreProvider
    {
        IMetricsDataStore GetDataStore();
    }
}