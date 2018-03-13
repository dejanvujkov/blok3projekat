using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Disconnector : Switch
    {
	  public Disconnector(long globalId) : base(globalId)
	  {
	  }
    }
}
