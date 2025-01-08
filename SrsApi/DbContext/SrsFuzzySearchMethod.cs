using Microsoft.EntityFrameworkCore;
using SrsApi.DbModels;
using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.DbContext
{
    public class SrsFuzzySearchMethod
    {
        [Key]
        public virtual FuzzySearchMethod FuzzySearchMethod { get; set; }

        public virtual string FuzzySearchMethodName { get; set; }
        
    }
}
