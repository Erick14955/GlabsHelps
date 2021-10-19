namespace GlabsHelps.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Direccion = c.String(),
                        Telefono = c.String(nullable: false),
                        CorreoCliente = c.String(),
                        Contacto = c.String(),
                        CelularContacto = c.String(),
                        CorreoContacto = c.String(),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "dbo.Equipos",
                c => new
                    {
                        IdEquipo = c.Int(nullable: false, identity: true),
                        IdCliente = c.Int(nullable: false),
                        Descripcion = c.String(nullable: false),
                        Responsable = c.String(nullable: false),
                        DireccionAnyDesk = c.String(nullable: false),
                        DireccionTeamViewer = c.String(nullable: false),
                        IpEquipo = c.String(nullable: false),
                        IpPublica = c.String(nullable: false),
                        IpLocal = c.String(nullable: false),
                        TipoEquipo = c.String(),
                        UsuarioEquipo = c.String(),
                        ClaveEquipo = c.String(),
                    })
                .PrimaryKey(t => t.IdEquipo);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Equipos");
            DropTable("dbo.Clientes");
        }
    }
}
