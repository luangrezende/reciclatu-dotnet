namespace DATABASE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agendamentos", "dtAbertura", c => c.DateTime(nullable: false));
            DropColumn("dbo.Agendamentos", "dtAtual");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Agendamentos", "dtAtual", c => c.DateTime(nullable: false));
            DropColumn("dbo.Agendamentos", "dtAbertura");
        }
    }
}
