using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace obsApi.Migrations
{
    public partial class ajoutGeoloc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE [dbo].[AZObsItems] ADD [Location] geography NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
