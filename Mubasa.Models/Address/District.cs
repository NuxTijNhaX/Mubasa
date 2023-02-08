using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    [Table("Districts", Schema = "Address")]
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Id_Ghn { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
    }
}
