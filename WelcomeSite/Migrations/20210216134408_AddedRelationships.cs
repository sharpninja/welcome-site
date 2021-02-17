using Microsoft.EntityFrameworkCore.Migrations;

namespace WelcomeSite.Migrations
{
    public partial class AddedRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyResponses",
                table: "SurveyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyQuestions",
                table: "SurveyQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Respondents",
                table: "Respondents");

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Respondents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Preferneces",
                table: "Respondents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyResponse_ResponseId",
                table: "SurveyResponses",
                column: "RespondentID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyQuestion_QuestionId",
                table: "SurveyQuestions",
                column: "QuestionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Respondent_RespondentId",
                table: "Respondents",
                column: "RespondentID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_QuestionID",
                table: "SurveyResponses",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_Respondents_EmailAddress",
                table: "Respondents",
                column: "EmailAddress",
                unique: true,
                filter: "[EmailAddress] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponse_Respondent",
                table: "SurveyResponses",
                column: "RespondentID",
                principalTable: "Respondents",
                principalColumn: "RespondentID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponse_SurveyQuestion",
                table: "SurveyResponses",
                column: "QuestionID",
                principalTable: "SurveyQuestions",
                principalColumn: "QuestionID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponse_Respondent",
                table: "SurveyResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponse_SurveyQuestion",
                table: "SurveyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyResponse_ResponseId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_QuestionID",
                table: "SurveyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyQuestion_QuestionId",
                table: "SurveyQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Respondent_RespondentId",
                table: "Respondents");

            migrationBuilder.DropIndex(
                name: "IX_Respondents_EmailAddress",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Respondents");

            migrationBuilder.DropColumn(
                name: "Preferneces",
                table: "Respondents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyResponses",
                table: "SurveyResponses",
                column: "ResponseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyQuestions",
                table: "SurveyQuestions",
                column: "QuestionID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Respondents",
                table: "Respondents",
                column: "RespondentID");
        }
    }
}
