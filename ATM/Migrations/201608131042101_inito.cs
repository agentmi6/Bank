namespace ATM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inito : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transfers", "CheckingAccountNumberToTransfer", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transfers", "CheckingAccountNumberToTransfer", c => c.String());
        }
    }
}
