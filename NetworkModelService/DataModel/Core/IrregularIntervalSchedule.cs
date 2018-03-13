using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class IrregularIntervalSchedule : BasicIntervalSchedule
    {
	  public IrregularIntervalSchedule(long globalId) : base(globalId)
	  {
	  }

	  private List<long> timePoints = new List<long>();

	  public override int GetHashCode()
	  {
		return base.GetHashCode();
	  }

	  public override bool Equals(object x)
	  {
		IrregularIntervalSchedule s = (IrregularIntervalSchedule)x;
		if (base.Equals(x))
		{
		    return (s.timePoints == timePoints);
		}

		return false;
	  }

	  public override bool HasProperty(ModelCode property)
	  {
		switch (property)
		{
		    case ModelCode.IRREGULARINTERVALSCHEDULE_TIMEPOINTS:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }

	  public override void GetProperty(Property property)
	  {
		switch (property.Id)
		{
		    case ModelCode.IRREGULARINTERVALSCHEDULE_TIMEPOINTS:
			  property.SetValue(timePoints);
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
		    default:
			  base.SetProperty(property);
			  break;
		}
	  }

	  public override bool IsReferenced => timePoints.Count > 0 || base.IsReferenced;

	  public override void AddReference(ModelCode referenceId, long globalId)
	  {
		switch(referenceId)
		{
		    case ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE:
			  timePoints.Add(globalId);
			  break;
		    default:
			  base.AddReference(referenceId, globalId);
			  break;
		}
	  }

	  public override void RemoveReference(ModelCode referenceId, long globalId)
	  {
		switch (referenceId)
		{
		    case ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE:
			  if (!timePoints.Remove(globalId))
			  {
				CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
			  }
			  break;

		    default:
			  base.RemoveReference(referenceId, globalId);
			  break;
		}
	  }

	  public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
	  {
		if (timePoints.Count > 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
		{
		    references[ModelCode.IRREGULARINTERVALSCHEDULE_TIMEPOINTS] = new List<long>();
		    references[ModelCode.IRREGULARINTERVALSCHEDULE_TIMEPOINTS] = timePoints.GetRange(0, timePoints.Count);
		    return;
		}

		base.GetReferences(references, refType);
	  }
    }
}
