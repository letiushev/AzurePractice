using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTestJson
{
    internal class Todos
    {
        public int userId;
        public int id;
        public string title;
        public bool completed;

        public Todos()
        {
        }

        public Todos(int userId, int id, string title, bool completed)
        {
            this.userId = userId;
            this.id = id;
            this.title = title;
            this.completed = completed;
        }
    }
}
