namespace Emart.Migrations.People
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.E_Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Gender = c.String(),
                        Profession = c.String(),
                        PhoneNumber = c.Int(nullable: false),
                        Address = c.String(),
                        imgid = c.Int(nullable: false),
                        UserImageStore_ImageID = c.Int(),
                        UserPostImages_ImageID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserImageStores", t => t.UserImageStore_ImageID)
                .ForeignKey("dbo.UserPostImages", t => t.UserPostImages_ImageID)
                .Index(t => t.UserImageStore_ImageID)
                .Index(t => t.UserPostImages_ImageID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        Group_link = c.String(),
                        Group_image = c.String(),
                        join = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userid = c.Int(nullable: false),
                        postid = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Recommends",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userid = c.String(),
                        ProductName = c.String(),
                        quantity = c.Int(nullable: false),
                        unitprice = c.Double(),
                        country = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.S_Comments",
                c => new
                    {
                        comID = c.Int(nullable: false, identity: true),
                        Post_id = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                        Coment = c.String(),
                        Com_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.comID);
            
            CreateTable(
                "dbo.S_frns",
                c => new
                    {
                        frnID = c.Int(nullable: false, identity: true),
                        S_userID = c.String(),
                        s_frnID = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.frnID);
            
            CreateTable(
                "dbo.S_likes",
                c => new
                    {
                        likeID = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        userId = c.Int(nullable: false),
                        LikeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.likeID);
            
            CreateTable(
                "dbo.S_msgs",
                c => new
                    {
                        msgID = c.Int(nullable: false, identity: true),
                        msgtime = c.DateTime(nullable: false),
                        msg_SenderID = c.Int(nullable: false),
                        msg_RecpientID = c.Int(nullable: false),
                        msg_text = c.String(),
                        msg_Subject = c.String(),
                    })
                .PrimaryKey(t => t.msgID);
            
            CreateTable(
                "dbo.S_Post",
                c => new
                    {
                        postID = c.Int(nullable: false, identity: true),
                        S_userID = c.Int(nullable: false),
                        PostTime = c.String(),
                        PostContent = c.String(),
                        Post_like = c.Int(nullable: false),
                        Postimgid = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.postID);
            
            CreateTable(
                "dbo.UserImageStores",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        ImageByte = c.Binary(),
                        ImagePath = c.String(),
                        ImageisDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.ImageID);
            
            CreateTable(
                "dbo.UserPostImages",
                c => new
                    {
                        ImageID = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        ImageByte = c.Binary(),
                        ImagePath = c.String(),
                        ImageisDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.ImageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.E_Users", "UserPostImages_ImageID", "dbo.UserPostImages");
            DropForeignKey("dbo.E_Users", "UserImageStore_ImageID", "dbo.UserImageStores");
            DropIndex("dbo.E_Users", new[] { "UserPostImages_ImageID" });
            DropIndex("dbo.E_Users", new[] { "UserImageStore_ImageID" });
            DropTable("dbo.UserPostImages");
            DropTable("dbo.UserImageStores");
            DropTable("dbo.S_Post");
            DropTable("dbo.S_msgs");
            DropTable("dbo.S_likes");
            DropTable("dbo.S_frns");
            DropTable("dbo.S_Comments");
            DropTable("dbo.Recommends");
            DropTable("dbo.Notifications");
            DropTable("dbo.Groups");
            DropTable("dbo.E_Users");
        }
    }
}
