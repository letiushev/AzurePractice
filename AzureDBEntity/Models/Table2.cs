using System;
using System.Collections.Generic;

namespace AzureDBEntity.Models;

public partial class Table2
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string ErrorType { get; set; }
}
