using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mubasa.Models
{
    [Table("Provinces", Schema = "Address")]
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Id_Ghn { get; set; }
    }
}
