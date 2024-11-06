using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCTask.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT Id FROM AspNetRoles WHERE Id = 'D043078E-2A62-4A55-9267-1030ECDAB348')
                                BEGIN

                                INSERT INTO AspNetRoles
                                           (Id
                                           ,[Name]
                                           ,[NormalizedName])           
                                     VALUES
                                           ('D043078E-2A62-4A55-9267-1030ECDAB348', 'admin', 'ADMIN')
                                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM AspNetRoles WHERE Id = 'D043078E-2A62-4A55-9267-1030ECDAB348'");
        }
    }
}
