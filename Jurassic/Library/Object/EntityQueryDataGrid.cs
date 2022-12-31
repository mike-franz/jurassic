using System;
using System.Collections.Generic;
using System.Globalization;

namespace Jurassic.Library
{
    public sealed class EntityQueryDataGrid : IEntityQueryData
    {
        private readonly ScriptEngine _engine;
        private int currentRowIndex = -1;

        private readonly ObjectInstance _instance;
        private readonly ArrayInstance _dataRows;
        private ArrayInstance _currentRow; 

        public EntityQueryDataGrid(ScriptEngine engine)
        {
            _engine = engine;
            
            _instance = engine.Object.Construct();
            _instance["searchKey"] = engine.String.Construct(Guid.NewGuid().ToString());
            
            _dataRows = engine.Array.Construct();
            _instance["data"] = _dataRows;
        }
        
        public int Length => currentRowIndex + 1;

        public void StartNewRow()
        {
            currentRowIndex += 1;

            var newRow = _engine.Array.Construct();
            _dataRows.Push(newRow);
            _currentRow = newRow;
            _currentRow.Push($"{currentRowIndex}");
        }
        
        public void AddColumnNames(List<string> names)
        {
            var columnNamesProperty = _engine.Array.Construct();
            _instance["columnNames"] = columnNamesProperty;

            foreach (var columnName in names)
            {
                columnNamesProperty.Push(columnName);
            }
        }

        public void AddColumnData(object dataValue)
        {
            if (dataValue is DateTime dateTimeValue)
            {
                _currentRow.Push(dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
                return;
            }
            
            _currentRow.Push(dataValue);
        }

        public ObjectInstance ToObjectInstance()
        {
            return _instance;
        }
    }
}