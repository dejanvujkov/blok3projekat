using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class RegularIntervalSchedule : BasicIntervalSchedule
    {
	  public RegularIntervalSchedule(long globalId) : base(globalId)
	  {
	  }

	  private List<long> timePoints = new List<long>();

	  private DateTime endTime;

	  public DateTime EndTime
	  {
		get { return endTime; }
		set { endTime = value; }
	  }

	  private float timeStep;

	  public float TimeStep
	  {
		get { return timeStep; }
		set { timeStep = value; }
	  }

	  public override int GetHashCode()
	  {
		return base.GetHashCode();
	  }

	  public override bool Equals(object x)
	  {
		RegularIntervalSchedule r = (RegularIntervalSchedule)x;
		if(base.Equals(x))
		{
		    return (r.EndTime == endTime && r.TimeStep == timeStep && r.timePoints == timePoints);
		}
		return false;
	  }

	  public override bool HasProperty(ModelCode property)
	  {
		switch(property)
		{
		    case ModelCode.REGULARINTERVALSCHEDULE_ENDTIME:
		    case ModelCode.REGULARINTERVALSCHEDULE_TIMEPOINTS:
		    case ModelCode.REGULARINTERVALSCHEDULE_TIMESTEP:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }

	  public override void GetProperty(Property property)
	  {
		switch(property.Id)
		{
		    case ModelCode.REGULARINTERVALSCHEDULE_ENDTIME:
			  property.SetValue(endTime);
			  break;
		    case ModelCode.REGULARINTERVALSCHEDULE_TIMEPOINTS:
			  property.SetValue(timePoints);
			  break;
		    case ModelCode.REGULARINTERVALSCHEDULE_TIMESTEP:
			  property.SetValue(timeStep);
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
		    case ModelCode.REGULARINTERVALSCHEDULE_ENDTIME:
			  endTime = property.AsDateTime();
			  break;
		    case ModelCode.REGULARINTERVALSCHEDULE_TIMESTEP:
			  timeStep = property.AsFloat();
			  break;
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
		    case ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE:
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
		    case ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE:
			  if(!timePoints.Remove(globalId))
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
		    references[ModelCode.REGULARINTERVALSCHEDULE_TIMEPOINTS] = new List<long>();
		    references[ModelCode.REGULARINTERVALSCHEDULE_TIMEPOINTS] = timePoints.GetRange(0, timePoints.Count);
		    return;
		}

		base.GetReferences(references, refType);
	  }
    }
}
