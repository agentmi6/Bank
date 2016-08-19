namespace ATM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial25 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransferAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CheckingAccountNumberToTransfer = c.String(),
                        ConfirmTransfer = c.Boolean(nullable: false),
                        CheckingAccountID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CheckingAccounts", t => t.CheckingAccountID, cascadeDelete: true)
                .Index(t => t.CheckingAccountID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transfers", "CheckingAccountID", "dbo.CheckingAccounts");
            DropIndex("dbo.Transfers", new[] { "CheckingAccountID" });
            DropTable("dbo.Transfers");
        }
    }
}
