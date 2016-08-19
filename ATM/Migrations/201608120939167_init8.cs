namespace ATM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.qCashes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuickCash = c.Boolean(nullable: false),
                        CheckingAccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckingAccounts", t => t.CheckingAccountId, cascadeDelete: true)
                .Index(t => t.CheckingAccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.qCashes", "CheckingAccountId", "dbo.CheckingAccounts");
            DropIndex("dbo.qCashes", new[] { "CheckingAccountId" });
            DropTable("dbo.qCashes");
        }
    }
}
