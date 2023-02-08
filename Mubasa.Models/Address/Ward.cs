using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    [Table("Wards", Schema = "Address")]
    public class Ward
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Id_Ghn { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
    }
}
