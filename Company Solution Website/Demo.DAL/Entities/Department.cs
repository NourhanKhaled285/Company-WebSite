using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateOfCreation { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

    }
}
