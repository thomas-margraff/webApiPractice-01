using Microsoft.EntityFrameworkCore.Migrations;

namespace CoronaVirusDAL.Migrations
{
    public partial class createvwScrapeRunview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
                CREATE VIEW [dbo].[vwScrapeRun]
                AS
                SELECT          sr.Heading, sr.ScrapeDate, gg.Name AS GeoLocation, cc.Name AS Country, 
                                st.CaseCount AS Cases, st.DeathCount AS Deaths, st.Notes, sr.Id AS ScrapeRunId, 
                                gg.Id AS GeoLocationId, cc.Id AS CountryId, st.Id AS CountryStatsId, 
                                sr.CreateDate AS ScrapeCreateDate
                FROM            dbo.CountryStats AS st INNER JOIN
                                dbo.ScrapeRuns AS sr ON st.ScrapeRunId = sr.Id INNER JOIN
                                dbo.Countries AS cc ON st.CountryId = cc.Id INNER JOIN
                                dbo.GeoLocations AS gg ON cc.GeoLocationId = gg.Id
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[vwScrapeRun]");
        }
    }
}
