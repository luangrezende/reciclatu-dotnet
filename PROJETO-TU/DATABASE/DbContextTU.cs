using DATABASE.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;

namespace DATABASE
{
    public class DbContextTU : DbContext
    {
        public DbContextTU():base("DbContextTU") { }

        //CLASSES
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<Agendamentos> Agendamentos { get; set; }
        public virtual DbSet<Enderecos> Enderecos { get; set; }
        public virtual DbSet<TiposUsuario> TiposUsuarios { get; set; }
        public virtual DbSet<StatusAgendamento> StatusAgendamento { get; set; }
        public virtual DbSet<TipoMaterial> TipoMaterial { get; set; }

        //BUILDA CLASSES
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.Agendamentos)
                .WithRequired(e => e.UsuariosColeta)
                .HasForeignKey(e => e.idUsuarioColeta)
                .WillCascadeOnDelete(true)
                ;

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.Agendamentos)
                .WithRequired(e => e.UsuariosSolicita)
                .HasForeignKey(e => e.idUsuarioSolicita)
                .WillCascadeOnDelete(true)
                ;

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.Enderecos)
                .WithRequired(e => e.Usuarios)
                .HasForeignKey(e => e.IdUsuario)
                .WillCascadeOnDelete(true)
                ;

            modelBuilder.Entity<Enderecos>()
                .HasMany(e => e.Agendamentos)
                .WithRequired(e => e.Enderecos)
                .HasForeignKey(e => e.idEndereco)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<StatusAgendamento>()
                .HasMany(e => e.Agendamentos)
                .WithRequired(e => e.StatusAgendamento)
                .HasForeignKey(e => e.idStatus)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<TiposUsuario>()
                .HasMany(e => e.Usuarios)
                .WithRequired(e => e.TiposUsuario)
                .HasForeignKey(e => e.idTipoUsuario)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<TipoMaterial>()
                .HasMany(e => e.Agendamentos)
                .WithRequired(e => e.TipoMaterial)
                .HasForeignKey(e => e.idTipoMaterial)
                .WillCascadeOnDelete(false)
                ;

            modelBuilder.Entity<Agendamentos>()
              .Property(e => e.dtAgendamento)
              .HasColumnName("dtAgendamento")
              .HasColumnType("datetime")
              ;
        }
    }
}