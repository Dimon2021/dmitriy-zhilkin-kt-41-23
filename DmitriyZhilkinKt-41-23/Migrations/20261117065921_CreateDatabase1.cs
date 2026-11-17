using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DmitriyZhilkinKt_41_23.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Disciplines_DisciplineId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Specialties_SpecialityId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Groups_GroupId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Specialties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Grades",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Disciplines",
                table: "Disciplines");

            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "Students",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Students",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Students",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Students",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Students",
                newName: "student_id");

            migrationBuilder.RenameIndex(
                name: "IX_Students_GroupId",
                table: "Students",
                newName: "idx_students_fk_group_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Groups",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Course",
                table: "Groups",
                newName: "course");

            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "Groups",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Groups",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "SpecialityId",
                table: "Groups",
                newName: "specialty_id");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_SpecialityId",
                table: "Groups",
                newName: "idx_groups_fk_specialty_id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Grades",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Grades",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "DisciplineId",
                table: "Grades",
                newName: "discipline_id");

            migrationBuilder.RenameColumn(
                name: "GradeId",
                table: "Grades",
                newName: "grade_id");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_StudentId",
                table: "Grades",
                newName: "idx_grades_fk_student_id");

            migrationBuilder.RenameIndex(
                name: "IX_Grades_DisciplineId",
                table: "Grades",
                newName: "idx_grades_fk_discipline_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Disciplines",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "Disciplines",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DisciplineId",
                table: "Disciplines",
                newName: "discipline_id");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "Students",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "Groups",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "Disciplines",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "pk_Students_student_id",
                table: "Students",
                column: "student_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_Groups_group_id",
                table: "Groups",
                column: "group_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_Grades_grade_id",
                table: "Grades",
                column: "grade_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_Disciplines_discipline_id",
                table: "Disciplines",
                column: "discipline_id");

            migrationBuilder.CreateTable(
                name: "Specialtys",
                columns: table => new
                {
                    specialty_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_Specialtys_specialty_id", x => x.specialty_id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_grades_discipline_id",
                table: "Grades",
                column: "discipline_id",
                principalTable: "Disciplines",
                principalColumn: "discipline_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_grades_student_id",
                table: "Grades",
                column: "student_id",
                principalTable: "Students",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_groups_specialty_id",
                table: "Groups",
                column: "specialty_id",
                principalTable: "Specialtys",
                principalColumn: "specialty_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_students_group_id",
                table: "Students",
                column: "group_id",
                principalTable: "Groups",
                principalColumn: "group_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_grades_discipline_id",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "fk_grades_student_id",
                table: "Grades");

            migrationBuilder.DropForeignKey(
                name: "fk_groups_specialty_id",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "fk_students_group_id",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Specialtys");

            migrationBuilder.DropPrimaryKey(
                name: "pk_Students_student_id",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "pk_Groups_group_id",
                table: "Groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_Grades_grade_id",
                table: "Grades");

            migrationBuilder.DropPrimaryKey(
                name: "pk_Disciplines_discipline_id",
                table: "Disciplines");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Students",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Students",
                newName: "isDeleted");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "Students",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Students",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "Students",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "idx_students_fk_group_id",
                table: "Students",
                newName: "IX_Students_GroupId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Groups",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "course",
                table: "Groups",
                newName: "Course");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Groups",
                newName: "isDeleted");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "Groups",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "specialty_id",
                table: "Groups",
                newName: "SpecialityId");

            migrationBuilder.RenameIndex(
                name: "idx_groups_fk_specialty_id",
                table: "Groups",
                newName: "IX_Groups_SpecialityId");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Grades",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "Grades",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "discipline_id",
                table: "Grades",
                newName: "DisciplineId");

            migrationBuilder.RenameColumn(
                name: "grade_id",
                table: "Grades",
                newName: "GradeId");

            migrationBuilder.RenameIndex(
                name: "idx_grades_fk_student_id",
                table: "Grades",
                newName: "IX_Grades_StudentId");

            migrationBuilder.RenameIndex(
                name: "idx_grades_fk_discipline_id",
                table: "Grades",
                newName: "IX_Grades_DisciplineId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Disciplines",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Disciplines",
                newName: "isDeleted");

            migrationBuilder.RenameColumn(
                name: "discipline_id",
                table: "Disciplines",
                newName: "DisciplineId");

            migrationBuilder.AlterColumn<bool>(
                name: "isDeleted",
                table: "Students",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "isDeleted",
                table: "Groups",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "isDeleted",
                table: "Disciplines",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Grades",
                table: "Grades",
                column: "GradeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Disciplines",
                table: "Disciplines",
                column: "DisciplineId");

            migrationBuilder.CreateTable(
                name: "Specialties",
                columns: table => new
                {
                    SpecialityId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.SpecialityId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Disciplines_DisciplineId",
                table: "Grades",
                column: "DisciplineId",
                principalTable: "Disciplines",
                principalColumn: "DisciplineId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_Students_StudentId",
                table: "Grades",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Specialties_SpecialityId",
                table: "Groups",
                column: "SpecialityId",
                principalTable: "Specialties",
                principalColumn: "SpecialityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Groups_GroupId",
                table: "Students",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
