using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electronic_WMS.Utilities.Enum
{
    public enum CommonStatus
    {
        IsDelete = 0,
        IsActive = 1,
    }

    public enum InventoryStatus 
    {
        IsDelete = 0,
        IsReady = 1,
        IsComplete = 2,
        IsCancle = 3,
    }

    public enum SeriStatus
    {
        IsDelete = 0,
        IsStock = 1,
        IsReleased = 2,
    }
}
