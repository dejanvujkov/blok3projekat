using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class BasicIntervalSchedule : IdentifiedObject
    {
	  public BasicIntervalSchedule(long globalId) : base(globalId)
	  {
	  }

	  private DateTime startTime;

	  public DateTime StartTime
	  {
		get { return startTime; }
		set { startTime = value; }
	  }

	  private UnitMultiplier v1Multiplier;

	  public UnitMultiplier V1Multiplier
	  {
		get { return v1Multiplier; }
		set { v1Multiplier = value; }
	  }

	  private UnitSymbol v1Unit;

	  public UnitSymbol V1Unit
	  {
		get { return v1Unit; }
		set { v1Unit = value; }
	  }

	  private UnitMultiplier v2Multiplier;

	  public UnitMultiplier V2Multiplier
	  {
		get { return v2Multiplier; }
		set { v2Multiplier = value; }
	  }

	  private UnitSymbol v2Unit;

	  public UnitSymbol V2Unit
	  {
		get { return v2Unit; }
		set { v2Unit = value; }
	  }

	  public override int GetHashCode()
	  {
		return base.GetHashCode();
	  }

	  public override bool Equals(object x)
	  {
		BasicIntervalSchedule b = (BasicIntervalSchedule)x;
		if(base.Equals(x))
		{
		    return (b.StartTime == startTime && b.V1Multiplier == v1Multiplier && b.V1Unit == v1Unit && b.V2Multiplier == v2Multiplier && b.V2Unit == v2Unit);
		}

		return false;
	  }

	  public override bool HasProperty(ModelCode property)
	  {
		switch(property)
		{
		    case ModelCode.BINTERVALSCHEDULE_STARTTIME:
		    case ModelCode.BINTERVALSCHEDULE_V1MULTIPLIER:
		    case ModelCode.BINTERVALSCHEDULE_V1UNIT:
		    case ModelCode.BINTERVALSCHEDULE_V2MULTIPLIER:
		    case ModelCode.BINTERVALSCHEDULE_V2UNIT:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }

	  public override void GetProperty(Property property)
	  {
		switch(property.Id)
		{
		    case ModelCode.BINTERVALSCHEDULE_STARTTIME:
			  property.SetValue(startTime);
			  break;
		    case ModelCode.BINTERVALSCHEDULE_V1MULTIPLIER:
			  property.SetValue((short)v1Multiplier);
			  break;
		    case ModelCode.BINTERVALSCHEDULE_V1UNIT:
			  property.SetValue((short)v1Unit);
			  break;
		    case ModelCode.BINTERVALSCHEDULE_V2MULTIPLIER:
			  property.SetValue((short)v2Multiplier);
			  break;
		    case ModelCode.BINTERVALSCHEDULE_V2UNIT:
			  property.SetValue((short)v2Unit);
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
		    case ModelCode.BINTERVALSCHEDULE_STARTTIME:
			  startTime = property.AsDateTime();
			  break;
		    case ModelCode.BINTERVALSCHEDULE_V1MULTIPLIER:
			  v1Multiplier = (UnitMultiplier)property.AsEnum();
			  break;
		    case ModelCode.BINTERVALSCHEDULE_V1UNIT:
			  v1Unit = (UnitSymbol)property.AsEnum();
			  break;
		    case ModelCode.BINTERVALSCHEDULE_V2MULTIPLIER:
			  v2Multiplier = (UnitMultiplier)property.AsEnum();
			  break;
		    case ModelCode.BINTERVALSCHEDULE_V2UNIT:
			  v2Unit = (UnitSymbol)property.AsEnum();
			  break;
		    default:
			  base.SetProperty(property);
			  break;
		}
	  }
    }
}
