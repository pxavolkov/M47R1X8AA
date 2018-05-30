namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gift : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gifts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        UsedUserID = c.String(),
                        CreditsBonus = c.Int(nullable: false),
                        UsedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Gifts");
        }
    }
}
