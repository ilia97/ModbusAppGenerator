using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class DataTypeEntity
    {
        public int Id { set; get; }

        public ModbusDataType Type { set; get; }
        
        public int SlaveActionEntityId { set; get; }
        public SlaveActionEntity SlaveAction { set; get; }
    }
}
