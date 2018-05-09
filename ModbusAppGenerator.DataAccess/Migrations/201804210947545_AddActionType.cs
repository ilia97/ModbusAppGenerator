namespace ModbusAppGenerator.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SlaveActionEntities", "ActionType", c => c.Int(nullable: false));
            AddColumn("dbo.SlaveActionEntities", "Formula", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SlaveActionEntities", "Formula");
            DropColumn("dbo.SlaveActionEntities", "ActionType");
        }
    }
}
