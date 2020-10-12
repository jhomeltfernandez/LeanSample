namespace jtf_Project.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kajshdjkasdh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactNumber = c.String(),
                        Deleted = c.Boolean(),
                        DateHired = c.DateTime(),
                        DateOut = c.DateTime(),
                        Created = c.DateTime(),
                        Modified = c.DateTime(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        Age = c.Int(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        Address = c.String(),
                        ImageId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserImages", t => t.ImageId)
                .Index(t => t.ImageId);
            
            
            
            
            CreateTable(
                "dbo.OtherExpences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DestinationId = c.Int(nullable: false),
                        TruckId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DriverId = c.Int(nullable: false),
                        HelperCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WaterCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Destinations", t => t.DestinationId, cascadeDelete: true)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.Trucks", t => t.TruckId, cascadeDelete: true)
                .Index(t => t.DestinationId)
                .Index(t => t.TruckId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Trucks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Capacity = c.Decimal(precision: 18, scale: 2),
                        DriverId = c.Int(),
                        PlateNumber = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId)
                .Index(t => t.DriverId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RateId = c.Int(nullable: false),
                        Gross = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FuelCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Less = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Net = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rates", t => t.RateId, cascadeDelete: true)
                .Index(t => t.RateId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sales", "RateId", "dbo.Rates");
            DropForeignKey("dbo.Rates", "TruckId", "dbo.Trucks");
            DropForeignKey("dbo.Trucks", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Rates", "DriverId", "dbo.Drivers");
            DropForeignKey("dbo.Rates", "DestinationId", "dbo.Destinations");
            DropForeignKey("dbo.Drivers", "ImageId", "dbo.UserImages");
            DropIndex("dbo.Sales", new[] { "RateId" });
            DropIndex("dbo.Trucks", new[] { "DriverId" });
            DropIndex("dbo.Rates", new[] { "DriverId" });
            DropIndex("dbo.Rates", new[] { "TruckId" });
            DropIndex("dbo.Rates", new[] { "DestinationId" });
            DropIndex("dbo.Drivers", new[] { "ImageId" });
            DropTable("dbo.Sales");
            DropTable("dbo.Trucks");
            DropTable("dbo.Rates");
            DropTable("dbo.OtherExpences");
            DropTable("dbo.Drivers");
            DropTable("dbo.Destinations");
        }
    }
}
