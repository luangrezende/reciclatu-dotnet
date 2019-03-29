namespace DATABASE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v72 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Agendamentos", "dtAgendamento", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Agendamentos", "dtAgendamento", c => c.DateTime());
        }
    }
}
