using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDemo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            string spClearDB = @"CREATE PROCEDURE [dbo].[spClearDB]
                                AS
                                 BEGIN
	                                TRUNCATE TABLE [dbo].[Orders]

	                                IF (OBJECT_ID('FK_Orders_Products_ProductId', 'F') IS NOT NULL)
	                                BEGIN
                                    ALTER TABLE Orders DROP CONSTRAINT FK_Orders_Products_ProductId
	                                END

	                                TRUNCATE TABLE [dbo].[Products]

	                                ALTER TABLE [dbo].[Orders]
                                    ADD CONSTRAINT [FK_Orders_Products_ProductId] 
                                    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE;
                                 END";

            string spDeleteOrders = @"CREATE PROCEDURE [dbo].[spDeleteOrders]
                                        @Year INTEGER = NULL,
                                            @Month INTEGER = NULL,
                                        @Status nvarchar(20) = NULL,
                                        @Product INTEGER = NULL
                                        AS
                                            BEGIN
                                        SET NOCOUNT ON
                                            BEGIN TRANSACTION
                                            BEGIN TRY

                                            DELETE FROM Orders

                                        WHERE(@Year IS NULL OR YEAR(Orders.createdDate) = @Year)

                                        AND(@Month IS NULL OR Month(Orders.createdDate) = @Month)

                                        AND(@Status IS NULL OR Orders.status = @Status)

                                        AND(@Product IS NULL OR Orders.productId = @Product)

                                        COMMIT TRANSACTION
                                        END TRY
                                        BEGIN CATCH
                                        ROLLBACK TRANSACTION
                                        PRINT 'Error occurred during delete operation.'
                                        END CATCH
                                        END";

            string spGetOrdersByFilter = @"CREATE PROCEDURE [dbo].[spGetOrdersByFilter]
	                                    @Year INTEGER = NULL,
	                                    @Month INTEGER = NULL,
	                                    @Status nvarchar(20) = NULL,
	                                    @Product INTEGER = NULL
                                        AS
	                                    BEGIN
		                                    SET NOCOUNT ON;
		                                    SELECT * FROM Orders
		                                    WHERE (@Year IS NULL OR YEAR(Orders.createdDate) = @Year)
		                                    AND (@Month IS NULL OR Month(Orders.createdDate) = @Month)
		                                    AND (@Status IS NULL OR Orders.status = @Status)
		                                    AND (@Product IS NULL OR Orders.productId = @Product)
	                                    END";

            migrationBuilder.Sql(spClearDB);
            migrationBuilder.Sql(spDeleteOrders);
            migrationBuilder.Sql(spGetOrdersByFilter);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.Sql("DROP PROCEDURE [dbo].[spClearDB]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[spDeleteOrders]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[spGetOrdersByFilter]");
        }
    }
}
