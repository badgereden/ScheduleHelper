using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleHelper
{
    /// <summary>
    /// 尝试安排初始化课表失败
    /// </summary>
    public class AssignFailedException : SystemException
    {
        public AssignFailedException(string msg)
            : base(msg)
        {
        }

    }

}
