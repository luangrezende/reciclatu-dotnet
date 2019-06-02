namespace DATABASE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agendamentos", "vizualizado", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agendamentos", "vizualizado");
        }
    }
}
