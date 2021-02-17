using Microsoft.EntityFrameworkCore.Migrations;

namespace WelcomeSite.Migrations
{
    public partial class FixKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyResponse_ResponseId",
                table: "SurveyResponses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyResponse_ResponseId",
                table: "SurveyResponses",
                column: "ResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_RespondentID",
                table: "SurveyResponses",
                column: "RespondentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyResponse_ResponseId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_RespondentID",
                table: "SurveyResponses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyResponse_ResponseId",
                table: "SurveyResponses",
                column: "RespondentID");
        }
    }
}
