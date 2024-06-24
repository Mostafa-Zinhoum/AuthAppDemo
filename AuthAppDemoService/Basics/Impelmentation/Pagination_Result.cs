using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthAppDemoService.Basics.Impelmentation
{
    public class Pagination_Result
    {
        public int Page_Index { get; private set; }
        public int Page_Count { get; private set; }
        public Int64 Total_Count { get; private set; }
        public int Total_Pages_Count { get; private set; }
        public object Data { get; private set; }
        public bool Has_Previous_Page
        {

            get
            {
                return (Page_Index > 1);
            }
        }

        public bool Has_Next_Page
        {

            get
            {
                return (Page_Index < Total_Pages_Count);
            }
        }

        public Pagination_Result(object source, int PageIndex, int PageCount, Int64 TotalCount)
        {

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            Data = source;
            Page_Index = PageIndex;
            Page_Count = PageCount;
            Total_Count = TotalCount;
            if (Total_Count == 0)
            {
                Total_Pages_Count = 0;
            }
            else
            {
                Total_Pages_Count = (int)Math.Ceiling(Total_Count / (double)Page_Count);
            }

        }
    }
}
