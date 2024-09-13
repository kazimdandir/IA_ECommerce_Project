using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Repositories.Migrations
{
    public partial class testMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SizeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailsCare = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Products_ProductId1",
                        column: x => x.ProductId1,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingCartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FullName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "840c569c-bbf5-4697-b3b4-ba24d863fea4", "johndoe@example.com", true, "John Doe", false, null, "JOHNDOE@EXAMPLE.COM", "JOHNDOE", "AQAAAAEAACcQAAAAEDDWhzxKurHEF0/gu5tqDKRPmvxL6Zqo350GfyvY7RJmw8dW7wJjbT4aBWaNXKV4mg==", null, false, 0, "c6e0d403-dd11-48cc-904d-8463fb52bf60", false, "johndoe" },
                    { "2", 0, "5b7e4ca3-6cd5-42df-acef-7af3f4cccb8b", "janesmith@example.com", true, "Jane Smith", false, null, "JANESMITH@EXAMPLE.COM", "JANESMITH", "AQAAAAEAACcQAAAAED+R1Kgmgv2MtL2jdciUOiAi1N0w9qYwKMyWyY2xwVirXyE+uw4cU/vvDknIj0KSVQ==", null, false, 0, "3afc6a51-5a2a-496c-8b2a-dca3e1ed2c29", false, "janesmith" },
                    { "3", 0, "11c16693-a7b9-4844-b3ab-daafed88fb37", "kikbal.dandir@gmail.com", true, "Kazım İkbal Dandır", false, null, "KAZIMIKBALDANDIR@GMAIL.COM", "KAZIMIKBALDANDIR", "Kazim.123", null, false, 0, "184e318e-e773-4709-92aa-814ac0735066", false, "kazimdandir" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Footwear" },
                    { 2, "Jewellery" },
                    { 3, "Hats & Caps" },
                    { 4, "Sunglasses" },
                    { 5, "T-Shirts" },
                    { 6, "Tracksuits" },
                    { 7, "Jeans" },
                    { 8, "Hoodies & Sweatshirts" },
                    { 9, "Coats & Jackets" },
                    { 10, "Trousers" },
                    { 11, "Shorts" },
                    { 12, "Shirts" },
                    { 13, "Joggers" },
                    { 14, "Cargos" }
                });

            migrationBuilder.InsertData(
                table: "Sizes",
                columns: new[] { "Id", "SizeName" },
                values: new object[,]
                {
                    { 1, "28" },
                    { 2, "32" },
                    { 3, "34" },
                    { 4, "36" },
                    { 5, "38" },
                    { 6, "40" },
                    { 7, "42" },
                    { 8, "44" },
                    { 9, "46" },
                    { 10, "XS" },
                    { 11, "S" },
                    { 12, "M" },
                    { 13, "L" },
                    { 14, "XL" },
                    { 15, "XXL" },
                    { 16, "XXXL" },
                    { 17, "One Size" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "DetailsCare", "ImagePath", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 5, null, "100% cotton. Model is 6\"1 and wears a size 3XL", "/img/Oversized Bird Graphic T-shirt.png", "Oversized Bird Graphic T-shirt", 14m },
                    { 2, 5, null, "100% Baumwolle. Das Model ist 185cm groß und trägt Größe M", "/img/Oversized Deadpool Cereal License Print T-shirt.png", "Oversized Deadpool Cereal License Print T-shirt", 20m },
                    { 3, 5, null, "100% Baumwolle", "/img/Chocolate Oversized Extended Neck ABODE T-shirt.png", "Chocolate Oversized Extended Neck ABODE T-shirt", 14m },
                    { 4, 7, null, "100% COTTON. MODEL IS 6'1 AND WEARS SIZE 32.", "/img/Baggy Rigid Jean.png", "Baggy Rigid Jean", 25m },
                    { 5, 7, null, "100% COTTON. MODEL IS 6'1 AND WEARS SIZE 32.", "/img/Chocolate Relaxed Rigid Flare Patchwork Jeans.png", "Chocolate Relaxed Rigid Flare Patchwork Jeans", 42m },
                    { 6, 7, null, "100% Baumwolle. Das Model ist 185cm groß und trägt Größe 32.", "/img/Grey Slim Flared All Over Ripped Jeans With Let Down Hem.png", "Grey Slim Flared All Over Ripped Jeans With Let Down Hem", 35m },
                    { 7, 12, null, "50% Viskose, 40% Baumwolle, 10% Leinen. Das Model ist 185cm groß und trägt Größe M", "/img/Short Sleeve Linen Shirt.png", "Short Sleeve Linen Shirt", 50m },
                    { 8, 12, null, "100% Polyester. Model ist 185cm geoß und trägt Größe M.", "/img/Green Satin Oversized Revere Statue Border Shirt.png", "Green Satin Oversized Revere Statue Border Shirt", 28m },
                    { 9, 1, null, "Upper: Polyurethane (PU) Lining: Textile Insole: Textile Outsole: Thermoplastic Rubber (TPR)", "/img/Tonal Chunky Trainers In Blue.png", "Tonal Chunky Trainers In Blue", 35m },
                    { 10, 1, null, "UPPER/LINING : 100% PU SYNTHETIC, SOLE: 100% THERMOPLASTIC RUBBER", "/img/Stone Track Sole Loafer.png", "Stone Track Sole Loafer", 35m },
                    { 11, 1, null, "UPPER : 100% TEXTILE LINING : 100%PU SYNTH , SOCKS: 100%PU SYNTH, MID SOLE : 100% PU SOLE : 100% EVA", "/img/Red Tapestry Buckle Detail Mule.png", "Red Tapestry Buckle Detail Mule", 32m },
                    { 12, 2, null, "zinc alloy+glass", "/img/Silver Cuban Chain Jean Chain.png", "Silver Cuban Chain Jean Chain", 10m },
                    { 13, 2, null, "90%zinc alloy+8%glass+2%epoxy", "/img/Silver 3 Pack Mixed Bead Rings.png", "Silver 3 Pack Mixed Bead Rings", 8m },
                    { 14, 3, null, "80% Polyester, 20% Kunststoff", "/img/BM Flames Cap In Black.png", "BM Flames Cap In Black", 8m },
                    { 15, 3, null, "100% acrylic", "/img/Black Gothic Logo Jacquard Beanie.png", "Black Gothic Logo Jacquard Beanie", 12m },
                    { 16, 4, null, "90% Polycarbonate 10% Copper", "/img/Star Rimless Sunglasses In Red.png", "Star Rimless Sunglasses In Red", 8m },
                    { 17, 4, null, "90% Metal, 10% Plastic.", "/img/Brown Aviator Matte Sunglasses.png", "Brown Aviator Matte Sunglasses", 6m },
                    { 18, 6, null, "60% Cotton 40% Polyester. Model is 6'1 and wears size M.", "/img/Black Oversized Boxy Over The Seams Eagle Graphic Tracksuit.png", "Black Oversized Boxy Over The Seams Eagle Graphic Tracksuit", 45m },
                    { 19, 6, null, "60% Baumwolle 40% Polyester", "/img/Burgundy Oversized Boxy Cross Applique Zip Through Hoodie And Relaxed Jogger Tracksuit.png", "Burgundy Oversized Boxy Cross Applique Zip Through Hoodie And Relaxed Jogger Tracksuit", 60m },
                    { 20, 8, null, "50% Baumwolle, 50% Polyester", "/img/Sage Oversized Boxy ABODE Hoodie.png", "Sage Oversized Boxy ABODE Hoodie", 25m },
                    { 21, 8, null, "50% Baumwolle und 50% Polyester, das Model ist 185cm groß und trägt Größe M.", "/img/Sand Monaco Back Print Sweatshirt.png", "Sand Monaco Back Print Sweatshirt", 30m },
                    { 22, 9, null, "100% Polyurethane. Model is 6'1 and wears size M.", "/img/Yellow Oversized PU Badge Moto Jacket.png", "Yellow Oversized PU Badge Moto Jacket", 70m },
                    { 23, 9, null, "100% Baumwolle. Model ist 185cm groß und trägt Größe M", "/img/Washed black Oversized Dirty Wash Carpenter Denim Biker Jacket.png", "Washed black Oversized Dirty Wash Carpenter Denim Biker Jacket", 40m },
                    { 24, 10, null, "100% Cotton. Model is 6'1 and wears size M/ 32.", "/img/Stone Fixed Waist Relaxed Applique Print Trouser.png", "Stone Fixed Waist Relaxed Applique Print Trouser", 35m },
                    { 25, 10, null, "100% Cotton. Model is 6'1 and wears size M.", "/img/Slate Elasticated Waist Relaxed Fit Buckle Cargo Trouser.png", "Slate Elasticated Waist Relaxed Fit Buckle Cargo Trouser", 35m },
                    { 26, 11, null, "98% Baumwolle, 2% Elasthan. Model ist 185cm groß und trägt Größe M.", "/img/Grey Slim Fit Elasticated Waist Cargo Shorts.png", "Grey Slim Fit Elasticated Waist Cargo Shorts", 22m },
                    { 27, 11, null, "52% Polyester, 48% Baumwolle. Model ist 185cm groß und trägt Größe M", "/img/Charcoal Oversized Drop Crotch Rib Hem Loopback Short.png", "Charcoal Oversized Drop Crotch Rib Hem Loopback Short", 20m },
                    { 28, 13, null, "60% Baumwolle, 40% Polyester. Model ist 185cm groß und trägt EU Größe M.", "/img/Blue Relaxed Fit Split Hem Jacquard Joggers.png", "Blue Relaxed Fit Split Hem Jacquard Joggers", 30m },
                    { 29, 13, null, "80% cotton 20% polyester. Model is 6\\'1 and wears a size 3XL", "/img/Mint Plus Oversized Heavy Washed Applique Jogger.png", "Mint Plus Oversized Heavy Washed Applique Jogger", 30m },
                    { 30, 14, null, "98% Baumwolle, 2% Elastan. Das Model ist 185cm groß und trägt Größe 32", "/img/Olive Fixed Waist Slim Stacked Flare Strap Cargo Trouser.png", "Olive Fixed Waist Slim Stacked Flare Strap Cargo Trouser", 40m },
                    { 31, 14, null, "100% cotton. This model is 6\"1 and wears a size 3XL.", "/img/Grey Plus Fixed Waist Straight Fit Cargo Trousers.png", "Grey Plus Fixed Waist Straight Fit Cargo Trousers", 30m }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "Id", "UserId" },
                values: new object[,]
                {
                    { 1, "1" },
                    { 2, "2" }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCartItems",
                columns: new[] { "Id", "ProductId", "Quantity", "ShoppingCartId", "SizeId" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, 12 },
                    { 2, 2, 2, 1, 11 },
                    { 3, 3, 1, 2, 13 },
                    { 4, 4, 3, 2, 3 },
                    { 5, 5, 1, 1, 4 },
                    { 6, 6, 2, 2, 5 },
                    { 7, 7, 5, 1, 10 },
                    { 8, 8, 1, 2, 14 },
                    { 9, 9, 2, 2, 6 },
                    { 10, 10, 1, 1, 7 },
                    { 11, 11, 6, 1, 8 },
                    { 12, 12, 1, 1, 17 },
                    { 13, 13, 2, 2, 17 },
                    { 14, 14, 5, 2, 11 },
                    { 15, 15, 3, 1, 12 },
                    { 16, 16, 2, 1, 17 },
                    { 17, 17, 1, 2, 17 },
                    { 18, 18, 2, 1, 4 },
                    { 19, 19, 4, 2, 2 },
                    { 20, 20, 2, 1, 14 },
                    { 21, 21, 1, 2, 16 },
                    { 22, 22, 2, 1, 12 },
                    { 23, 23, 1, 2, 13 },
                    { 24, 24, 2, 2, 9 },
                    { 25, 25, 4, 1, 6 },
                    { 26, 26, 2, 1, 12 },
                    { 27, 27, 5, 2, 10 },
                    { 28, 28, 2, 1, 1 },
                    { 29, 29, 1, 2, 2 },
                    { 30, 30, 1, 1, 3 },
                    { 31, 31, 3, 1, 14 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ApplicationUserId",
                table: "FavoriteProducts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ProductId",
                table: "FavoriteProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ProductId1",
                table: "FavoriteProducts",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_UserId",
                table: "FavoriteProducts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ProductId",
                table: "ShoppingCartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ShoppingCartId",
                table: "ShoppingCartItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_SizeId",
                table: "ShoppingCartItems",
                column: "SizeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_UserId",
                table: "ShoppingCarts",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "FavoriteProducts");

            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
