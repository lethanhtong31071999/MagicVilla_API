using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace MagicVilla_VillaAPI.Models
{
    public class VillaNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        [ForeignKey("Villa")]
        public int VillaId { get; set; }
        [ValidateNever]
        public virtual Villa Villa { get; set; }
    }
}
