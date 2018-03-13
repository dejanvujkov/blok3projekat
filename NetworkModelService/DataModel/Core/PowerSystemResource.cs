using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FTN.Common;



namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class PowerSystemResource : IdentifiedObject
    {
	  public PowerSystemResource(long globalId) : base(globalId)
	  {
	  }

	  private long outage;

	  public long Outage
	  {
		get { return outage; }
		set { outage = value; }
	  }

	  public override bool Equals(object x)
	  {
		if (base.Equals(x))
		{
		    PowerSystemResource s = (PowerSystemResource)x;
		    return (s.Outage == outage);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		return base.GetHashCode();
	  }

	  public override bool HasProperty(ModelCode property)
	  {
		switch (property)
		{
		    case ModelCode.PSR_OUTAGESCHEDULE:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }

	  public override void GetProperty(Property property)
	  {
		switch (property.Id)
		{
		    case ModelCode.PSR_OUTAGESCHEDULE:
			  property.SetValue(outage);
			  break;
		    default:
			  base.GetProperty(property);
			  break;
		}
	  }

	  public override void SetProperty(Property property)
	  {
		switch(property.Id)
		{
		    case ModelCode.PSR_OUTAGESCHEDULE:
			  outage = property.AsReference();
			  break;
		    default:
			  base.SetProperty(property);
			  break;
		}
	  }

	  public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
	  {
		if(outage != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
		{
		    references[ModelCode.PSR_OUTAGESCHEDULE] = new List<long>();
		    references[ModelCode.PSR_OUTAGESCHEDULE].Add(outage);
		    return;
		}
		base.GetReferences(references, refType);
	  }

    }
}

