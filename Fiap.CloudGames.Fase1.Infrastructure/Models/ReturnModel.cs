using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fiap.CloudGames.Fase1.Infrastructure.Models
{
    public class ReturnModel
    {
        public object[]? ActionUI { get; set; }

        public object[]? Context { get; set; }

        public object? Object { get; set; }

        public ResultModel[]? Result { get; set; }

        public int Status { get; set; }
    }
    public class ResultModel
    {
        public int MessageCode { get; set; }

        public string? LocalError { get; set; }

        public string? Message { get; set; }

        public string? InternMessage { get; set; }

        public int Status { get; set; }
    }
}
