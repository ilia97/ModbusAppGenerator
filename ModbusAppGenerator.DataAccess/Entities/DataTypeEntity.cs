using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ModbusAppGenerator.DataAccess.Enums;

namespace ModbusAppGenerator.DataAccess.Entities
{
    public class DataTypeEntity
    {
        [Key]
        public int Id { set; get; }

        public ModbusDataType Type { set; get; }

        [ForeignKey("SlaveAction")]
        public int SlaveActionEntityId { set; get; }
        public SlaveActionEntity SlaveAction { set; get; }
    }
}
