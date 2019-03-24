namespace DATABASE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Enderecos", "rua", c => c.String(nullable: false));
            AddColumn("dbo.Enderecos", "complemento", c => c.String());
            AddColumn("dbo.Enderecos", "cidade", c => c.String(nullable: false));
            AddColumn("dbo.Enderecos", "numero", c => c.Int(nullable: false));
            AddColumn("dbo.Enderecos", "CEP", c => c.Int(nullable: false));
            DropColumn("dbo.Enderecos", "endereco");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Enderecos", "endereco", c => c.String(nullable: false));
            DropColumn("dbo.Enderecos", "CEP");
            DropColumn("dbo.Enderecos", "numero");
            DropColumn("dbo.Enderecos", "cidade");
            DropColumn("dbo.Enderecos", "complemento");
            DropColumn("dbo.Enderecos", "rua");
        }
    }
}
