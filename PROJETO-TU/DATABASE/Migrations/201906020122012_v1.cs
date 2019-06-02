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
                        dtAgendamento = c.DateTime(nullable: false),
                        dtAbertura = c.DateTime(nullable: false),
                        hora = c.String(nullable: false),
                        idUsuarioSolicita = c.Int(nullable: false),
                        idUsuarioColeta = c.Int(nullable: false),
                        idStatus = c.Int(nullable: false),
                        idEndereco = c.Int(nullable: false),
                        idTipoMaterial = c.Int(nullable: false),
                        UsuariosColeta_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Enderecos", t => t.idEndereco)
                .ForeignKey("dbo.Usuarios", t => t.idUsuarioSolicita, cascadeDelete: true)
                .ForeignKey("dbo.StatusAgendamento", t => t.idStatus)
                .ForeignKey("dbo.TipoMaterial", t => t.idTipoMaterial)
                .ForeignKey("dbo.Usuarios", t => t.UsuariosColeta_ID)
                .Index(t => t.idUsuarioSolicita)
                .Index(t => t.idStatus)
                .Index(t => t.idEndereco)
                .Index(t => t.idTipoMaterial)
                .Index(t => t.UsuariosColeta_ID);
            
            CreateTable(
                "dbo.Enderecos",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Rua = c.String(nullable: false),
                        Complemento = c.String(),
                        Cidade = c.String(nullable: false),
                        Estado = c.String(nullable: false),
                        Pais = c.String(),
                        Numero = c.Int(nullable: false),
                        Bairro = c.Int(nullable: false),
                        CEP = c.String(nullable: false),
                        Descricao = c.String(nullable: false),
                        IdUsuario = c.Int(nullable: false),
                        IdStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Usuarios", t => t.IdUsuario, cascadeDelete: true)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        idTipoUsuario = c.Int(nullable: false),
                        userName = c.String(nullable: false),
                        password = c.String(nullable: false),
                        nome = c.String(nullable: false),
                        CPF = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TiposUsuario", t => t.idTipoUsuario)
                .Index(t => t.idTipoUsuario);
            
            CreateTable(
                "dbo.TiposUsuario",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        descricao = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.StatusAgendamento",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        descricao = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TipoMaterial",
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
            DropForeignKey("dbo.Agendamentos", "idTipoMaterial", "dbo.TipoMaterial");
            DropForeignKey("dbo.Agendamentos", "idStatus", "dbo.StatusAgendamento");
            DropForeignKey("dbo.Usuarios", "idTipoUsuario", "dbo.TiposUsuario");
            DropForeignKey("dbo.Enderecos", "IdUsuario", "dbo.Usuarios");
            DropForeignKey("dbo.Agendamentos", "idUsuarioSolicita", "dbo.Usuarios");
            DropForeignKey("dbo.Agendamentos", "idEndereco", "dbo.Enderecos");
            DropIndex("dbo.Usuarios", new[] { "idTipoUsuario" });
            DropIndex("dbo.Enderecos", new[] { "IdUsuario" });
            DropIndex("dbo.Agendamentos", new[] { "UsuariosColeta_ID" });
            DropIndex("dbo.Agendamentos", new[] { "idTipoMaterial" });
            DropIndex("dbo.Agendamentos", new[] { "idEndereco" });
            DropIndex("dbo.Agendamentos", new[] { "idStatus" });
            DropIndex("dbo.Agendamentos", new[] { "idUsuarioSolicita" });
            DropTable("dbo.TipoMaterial");
            DropTable("dbo.StatusAgendamento");
            DropTable("dbo.TiposUsuario");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Enderecos");
            DropTable("dbo.Agendamentos");
        }
    }
}
