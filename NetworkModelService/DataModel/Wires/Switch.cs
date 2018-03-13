using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Switch : ConductingEquipment
    {
	  public Switch(long globalId) : base(globalId)
	  {
	  }

	  private bool normallyOpen;

	  public bool NormallyOpen
	  {
		get { return normallyOpen; }
		set { normallyOpen = value; }
	  }

	  private float ratedCurrent;

	  public float RatedCurrent
	  {
		get { return ratedCurrent; }
		set { ratedCurrent = value; }
	  }

	  private bool retained;

	  public bool Retained
	  {
		get { return retained; }
		set { retained = value; }
	  }

	  private int switchedOnCount;

	  public int SwitchedOnCount
	  {
		get { return switchedOnCount; }
		set { switchedOnCount = value; }
	  }

	  private DateTime switchedOnDate;

	  public DateTime SwitchedOnDate
	  {
		get { return switchedOnDate; }
		set { switchedOnDate = value; }
	  }

	  public override int GetHashCode()
	  {
		return base.GetHashCode();
	  }

	  public override bool Equals(object obj)
	  {
		Switch s = (Switch)obj;
		if (base.Equals(obj))
		{
		    return (s.NormallyOpen == normallyOpen && s.RatedCurrent == ratedCurrent && s.Retained == retained && s.SwitchedOnCount == switchedOnCount && s.SwitchedOnDate == switchedOnDate);
		}

		return false;
	  }

	  public override bool HasProperty(ModelCode property)
	  {
		switch (property)
		{
		    case ModelCode.SWITCH_NORMOPEN:
		    case ModelCode.SWITCH_ONCOUNT:
		    case ModelCode.SWITCH_ONDATE:
		    case ModelCode.SWITCH_RATEDCURRENT:
		    case ModelCode.SWITCH_RETAINED:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }

	  public override void GetProperty(Property property)
	  {
		switch (property.Id)
		{
		    case ModelCode.SWITCH_NORMOPEN:
			  property.SetValue(normallyOpen);
			  break;
		    case ModelCode.SWITCH_ONCOUNT:
			  property.SetValue(switchedOnCount);
			  break;
		    case ModelCode.SWITCH_ONDATE:
			  property.SetValue(switchedOnDate);
			  break;
		    case ModelCode.SWITCH_RATEDCURRENT:
			  property.SetValue(ratedCurrent);
			  break;
		    case ModelCode.SWITCH_RETAINED:
			  property.SetValue(retained);
			  break;
		    default:
			  base.GetProperty(property);
			  break;
		}
	  }

	  public override void SetProperty(Property property)
	  {
		switch (property.Id)
		{
		    case ModelCode.SWITCH_NORMOPEN:
			  normallyOpen = property.AsBool();
			  break;
		    case ModelCode.SWITCH_ONCOUNT:
			  switchedOnCount = property.AsInt();
			  break;
		    case ModelCode.SWITCH_ONDATE:
			  switchedOnDate = property.AsDateTime();
			  break;
		    case ModelCode.SWITCH_RATEDCURRENT:
			  ratedCurrent = property.AsFloat();
			  break;
		    case ModelCode.SWITCH_RETAINED:
			  retained = property.AsBool();
			  break;
		    default:
			  base.SetProperty(property);
			  break;
		}
	  }

    }
}
