namespace DATABASE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agendamentos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        descricao = c.String(nullable: false),
                        idUsuarioSolicita = c.Int(nullable: false),
                        idUsuarioColeta = c.Int(nullable: false),
                        idStatus = c.Int(nullable: false),
                        idEndereco = c.Int(nullable: false),
                        UsuariosColeta_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Enderecos", t => t.idEndereco)
                .ForeignKey("dbo.Usuarios", t => t.idUsuarioSolicita, cascadeDelete: true)
                .ForeignKey("dbo.StatusAgendamentoes", t => t.idStatus)
                .ForeignKey("dbo.Usuarios", t => t.UsuariosColeta_ID)
                .Index(t => t.idUsuarioSolicita)
                .Index(t => t.idStatus)
                .Index(t => t.idEndereco)
                .Index(t => t.UsuariosColeta_ID);
            
            CreateTable(
                "dbo.Enderecos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        endereco = c.String(nullable: false),
                        descricao = c.String(nullable: false),
                        idUsuario = c.Int(nullable: false),
                        idStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Usuarios", t => t.idUsuario, cascadeDelete: true)
                .Index(t => t.idUsuario);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        idTipoUsuario = c.Int(nullable: false),
                        userName = c.String(nullable: false),
                        nome = c.String(nullable: false),
                        CPF = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TiposUsuarios", t => t.idTipoUsuario)
                .Index(t => t.idTipoUsuario);
            
            CreateTable(
                "dbo.TiposUsuarios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        descricao = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StatusAgendamentoes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        descricao = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Agendamentos", "UsuariosColeta_ID", "dbo.Usuarios");
            DropForeignKey("dbo.Agendamentos", "idStatus", "dbo.StatusAgendamentoes");
            DropForeignKey("dbo.Usuarios", "idTipoUsuario", "dbo.TiposUsuarios");
            DropForeignKey("dbo.Enderecos", "idUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.Agendamentos", "idUsuarioSolicita", "dbo.Usuarios");
            DropForeignKey("dbo.Agendamentos", "idEndereco", "dbo.Enderecos");
            DropIndex("dbo.Usuarios", new[] { "idTipoUsuario" });
            DropIndex("dbo.Enderecos", new[] { "idUsuario" });
            DropIndex("dbo.Agendamentos", new[] { "UsuariosColeta_ID" });
            DropIndex("dbo.Agendamentos", new[] { "idEndereco" });
            DropIndex("dbo.Agendamentos", new[] { "idStatus" });
            DropIndex("dbo.Agendamentos", new[] { "idUsuarioSolicita" });
            DropTable("dbo.StatusAgendamentoes");
            DropTable("dbo.TiposUsuarios");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Enderecos");
            DropTable("dbo.Agendamentos");
        }
    }
}
