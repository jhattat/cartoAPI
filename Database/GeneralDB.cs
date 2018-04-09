using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ObsApi.Models;

namespace obsApi.Database
{
    public static class GeneralDB
    {

        public static void UpdateLocation(DbContext ctx, string table, Location location, long id)
        {
            // replace , by .
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");


            string query = String.Format(@"UPDATE [dbo].[{0}] SET Location = geography::STPointFromText('POINT(' + CAST({1} AS VARCHAR(20)) + ' ' + CAST({2} AS VARCHAR(20)) + ')', 4326) WHERE(ID = {3})"
            , table.ToLower(), location.Longitude, location.Latitude, id);
            ctx.Database.ExecuteSqlCommand(query);
        }
        public static Location GetLocation(DbContext ctx, string table, long id)
        {
            Location location = new Location();

            using (var command = ctx.Database.GetDbConnection().CreateCommand())
            {
                string query = String.Format("SELECT Location.Lat AS Latitude, Location.Long AS Longitude FROM [dbo].[{0}] WHERE Id = {1}"
                    , table, id);
                command.CommandText = query;
                ctx.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    if (result.HasRows)
                    {
                        while (result.Read())
                        {
                            if (!result.IsDBNull(0) && !result.IsDBNull(1))
                            {
                                location.Latitude = result.GetDouble(0);
                                location.Longitude = result.GetDouble(1);
                            }
                        }

                    }
                }
            }

            return location;
        }
    }

}