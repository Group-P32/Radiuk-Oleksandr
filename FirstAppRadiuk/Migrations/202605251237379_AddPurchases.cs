namespace FirstAppRadiuk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPurchases : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Person = c.String(),
                        Address = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Purchases");
        }
    }
}
