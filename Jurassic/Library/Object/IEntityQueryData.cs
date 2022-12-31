using System.Collections.Generic;

namespace Jurassic.Library
{
    public interface IEntityQueryData
    {
        int Length { get; }
        
        ObjectInstance ToObjectInstance();

        void StartNewRow();

        void AddColumnNames(List<string> names);

        void AddColumnData(object dataValue);
    }
}