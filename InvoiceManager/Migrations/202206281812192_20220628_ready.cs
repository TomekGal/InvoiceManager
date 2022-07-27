namespace InvoiceManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20220628_ready : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "Comments", c => c.String());
            DropColumn("dbo.Invoices", "Coments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoices", "Coments", c => c.String());
            DropColumn("dbo.Invoices", "Comments");
        }
    }
}
