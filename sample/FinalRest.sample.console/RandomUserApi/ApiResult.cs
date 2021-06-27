using System;
using System.Collections.Generic;
using System.Text;

namespace BasicMvvm.Models.RandomUserApi
{
    public class ApiResult
    {
        public IEnumerable<ResultModel> Results { get; set; }
        public ApiResultInfo Info { get; set; }
    }
}
