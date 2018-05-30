using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ObsApi.Models
{
    public class AZObsItemDto : IValidatableObject
 {     

        public long Id { get; set; }
       public string Name { get; set; }
       public bool IsComplete { get; set; } 
       public double Longitude { get; set; }
       public double Latitude { get; set; }


     public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
     {
         if (String.IsNullOrEmpty(Name))
             yield return new ValidationResult("The Name Can Not Be Null");
               
     }
 }
}