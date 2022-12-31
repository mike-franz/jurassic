using System;
using System.Collections.Generic;
using System.Globalization;

namespace Jurassic.Library
{
    public sealed class EntityQueryDataObject : IEntityQueryData
    {
        private readonly ScriptEngine _engine;
        private int currentRowIndex = -1;
        private int currentColumnIndex = 0;
        
        private readonly ArrayInstance _instance;
        private ObjectInstance _currentRow;
        private List<string> columnNames;

        public EntityQueryDataObject(ScriptEngine engine)
        {
            _engine = engine;
            
            _instance = engine.Array.Construct();
        }
        
        public int Length => currentRowIndex + 1;

        public void StartNewRow()
        {
            currentRowIndex += 1;
            currentColumnIndex = -1;

            var newRow = _engine.Object.Construct();
            _instance.Push(newRow);
            _currentRow = newRow;
        }
        
        public void AddColumnNames(List<string> names)
        {
            columnNames = names;
            
            var columnNamesProperty = _engine.Array.Construct();
            _instance["columnNames"] = columnNamesProperty;

            foreach (var columnName in names)
            {
                columnNamesProperty.Push(columnName);
            }
        }

        public void AddColumnData(object dataValue)
        {
            currentColumnIndex += 1;

            var columnName = columnNames[currentColumnIndex];
            
            if (dataValue is DateTime dateTimeValue)
            {
                _currentRow[columnName] = dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
                return;
            }
            
            if (dataValue is long longValue)
            {
                _currentRow[columnName] = _engine.Number.Construct(longValue);
                return;
            }
            
            _currentRow[columnName] = (dataValue);
        }

        public ObjectInstance ToObjectInstance()
        {
            return _instance;
        }
    }
}