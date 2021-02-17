using Microsoft.EntityFrameworkCore.Migrations;

namespace WelcomeSite.Migrations
{
    public partial class NewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "QuestionOrder",
                table: "SurveyQuestions",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "QuestionTitle",
                table: "SurveyQuestions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionOrder",
                table: "SurveyQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionTitle",
                table: "SurveyQuestions");
        }
    }
}
