using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DataAccessLibrary.Models
{
    public static class MigrationBuilderExtention
    {
        public static void Seed(this MigrationBuilder migrationBuilder)
        {
            Guid[] companyIds = new Guid[5];
            
            // ================  Game Genres =====================
            migrationBuilder.Sql("Insert Into Genres Values('Action')", suppressTransaction: false);
            migrationBuilder.Sql("Insert Into Genres Values('Adventure')", suppressTransaction: false);
            migrationBuilder.Sql("Insert Into Genres Values('Action Adventure')", suppressTransaction: false);
            migrationBuilder.Sql("Insert Into Genres Values('Role Playing')", suppressTransaction: false);
            migrationBuilder.Sql("Insert Into Genres Values('Simulation')", suppressTransaction: false);
            migrationBuilder.Sql("Insert Into Genres Values('Strategy')", suppressTransaction: false);
            migrationBuilder.Sql("Insert Into Genres Values('Sports')", suppressTransaction: false);
            migrationBuilder.Sql("Insert Into Genres Values('MMO')", suppressTransaction: false);
            // ================   Develope Companies =====================
            for (int index =0; index < companyIds.Length; index++)
            {
                companyIds[index] = Guid.NewGuid();
            }

            migrationBuilder.Sql("Alter table Developers Add Constraint DF_Developers Default GetDate() For CreateDate", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Developers (ID, CompanyName,  Country, EstablishYear) " +
                "Values('" + companyIds[0] + "', 'Activision', 'United States', 1979)", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Developers (ID, CompanyName,  Country, EstablishYear) " +
                "Values('" + companyIds[1] + "', 'Electronic Arts', 'Canada', 1995)", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Developers (ID, CompanyName,  Country, EstablishYear) " +
                "Values('" + companyIds[2] + "', 'Criterion Games', 'United Kingdom', 1993)", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Developers (ID, CompanyName,  Country, EstablishYear) " +
                "Values('" + companyIds[3] + "', 'Epics', 'Japan', 1987)", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Developers (ID, CompanyName,  Country, EstablishYear) " +
                "Values('" + companyIds[4] + "', 'Aces Studio', 'Germany', 2007)", suppressTransaction: false);

            // ================   Catalogs =====================
            migrationBuilder.Sql("Alter table Catalogs Add Constraint DF_Catalogs Default GetDate() For CreateDate", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Catalogs (ID, Title, ReleaseDate, CompanyID, GenreID, Rate, Price) " +
                "Values('" + Guid.NewGuid() + "', 'Fifa20', '09/24/2019','" + companyIds[0] + "', 6, 5, 59.99)", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Catalogs (ID, Title, ReleaseDate, CompanyID, GenreID, Rate, Price) " +
                "Values('" + Guid.NewGuid() + "', 'Wing Of Fury I', '08/31/1994', '" + companyIds[1] + "', 1, 2, 59.99)", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Catalogs (ID, Title, ReleaseDate, CompanyID, GenreID, Rate, Price) " +
                "Values('" + Guid.NewGuid() + "', 'Call of Duty I', '10/29/2003', '" + companyIds[2] + "', 1, 5, 69.99)", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Catalogs (ID, Title, ReleaseDate, CompanyID, GenreID, Rate, Price) " +
                "Values('" + Guid.NewGuid() + "', 'Need For Speed I', '08/31/1994', '" + companyIds[3] + "', 3, 2, 2.66)", suppressTransaction: false);

            migrationBuilder.Sql("Insert Into Catalogs (ID, Title, ReleaseDate, CompanyID, GenreID, Rate, Price) " +
                "Values('" + Guid.NewGuid() + "', 'Flight Simulator X', '02/20/2006', '" + companyIds[4] + "', 5, 3, 12.55)", suppressTransaction: false);

        }
    }
}
