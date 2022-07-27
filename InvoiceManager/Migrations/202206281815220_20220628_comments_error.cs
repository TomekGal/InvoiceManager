namespace InvoiceManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20220628_comments_error : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invoices", "Coments", c => c.String());
            DropColumn("dbo.Invoices", "Comments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoices", "Comments", c => c.String());
            DropColumn("dbo.Invoices", "Coments");
        }
    }
}
