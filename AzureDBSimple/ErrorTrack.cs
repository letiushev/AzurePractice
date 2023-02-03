using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDB
{
    internal class ErrorTrack
    {
        public int Id;
        public int ProductId;
        public string ErrorType;
        public ErrorTrack(int id, int productId, string errorType)
        {
            this.Id = id;
            this.ProductId = productId;
            this.ErrorType = errorType;
        }
        public ErrorTrack()
        {
        }
    }
}
