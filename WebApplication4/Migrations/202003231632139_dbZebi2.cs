namespace WebApplication4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbZebi2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Armes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Degats = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Samourais",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Force = c.Int(nullable: false),
                        Nom = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Armes", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.ArtMartials",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Samourai_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Samourais", t => t.Samourai_Id)
                .Index(t => t.Samourai_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArtMartials", "Samourai_Id", "dbo.Samourais");
            DropForeignKey("dbo.Samourais", "Id", "dbo.Armes");
            DropIndex("dbo.ArtMartials", new[] { "Samourai_Id" });
            DropIndex("dbo.Samourais", new[] { "Id" });
            DropTable("dbo.ArtMartials");
            DropTable("dbo.Samourais");
            DropTable("dbo.Armes");
        }
    }
}
