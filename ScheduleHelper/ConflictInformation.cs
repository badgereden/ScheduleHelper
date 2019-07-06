using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    public class ConflictInformation
    {
        public int ConflictClassID { get; set; }
        public string SubjectName { get; set; }

        public ConflictInformation(int conflict_class_id, string subject_name)
        {
            ConflictClassID = conflict_class_id;
            SubjectName = subject_name;
        }
    }
}
