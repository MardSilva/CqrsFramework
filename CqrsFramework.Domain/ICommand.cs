using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CqrsFramework.CqrsFramework.Domain
{
    /// <summary>
    /// Default ICommand for command generation
    /// </summary>
    public interface ICommand 
    {
        string CommandName {  get; }
    }
}