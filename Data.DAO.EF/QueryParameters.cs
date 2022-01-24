using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.DAO.EF
{
    public class QueryParameters<T>
    {
        public QueryParameters()
        {

        }
        public QueryParameters(int _page, int _count)
        {

        }

        public int page { get; set; }
        public int count { get; set; }
        public Expression<Func<T, bool>> where { get; set; }
        public Func<T, object> orderBy { get; set; }
        public Func<T, object> orderByDesc { get; set; }
    }
}
