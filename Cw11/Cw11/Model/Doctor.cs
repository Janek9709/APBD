using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw11.Model
{
    public class Doctor
    {
        public virtual ICollection<Prescription> Prescriptions { get; set; }
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Doctor()
        {
            Prescriptions = new HashSet<Prescription>();
        }
    }
}