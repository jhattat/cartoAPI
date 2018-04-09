using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using Microsoft.EntityFrameworkCore;

namespace ObsApi.Models
{
    public class AZObsItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public ObsApi.Models.Location Geo { get; set; }

    }
}