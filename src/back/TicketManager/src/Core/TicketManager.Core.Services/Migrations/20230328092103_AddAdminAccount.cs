using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketManager.Core.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO \"Accounts\" (\"Id\", \"Email\", \"PasswordHash\", \"Role\")" +
                                 "VALUES ('bedebffa-73dc-4944-9727-4924d82dcbe2', 'admin@email.com', " +
                                 "'iGwhjRGxH2FEwca2gtikmjJojdRH7Xa+jxweKxukYKFz/Bgx', 'Admin');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
