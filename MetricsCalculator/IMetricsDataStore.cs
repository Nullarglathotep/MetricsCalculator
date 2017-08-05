namespace MetricsCalculator
{
    public interface IMetricsDataStore
    {
        bool TryGetDataItem<DataType>(string name, out DataType value);

        void SetDataItem<DataType>(string name, DataType value);
    }
}