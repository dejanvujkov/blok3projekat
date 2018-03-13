using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class IrregularTimePoint : IdentifiedObject
    {
	  public IrregularTimePoint(long globalId) : base(globalId)
	  {
	  }

	  private long intervalSchedule;

	  public long IntervalSchedule
	  {
		get { return intervalSchedule; }
		set { intervalSchedule = value; }
	  }

	  private float time;

	  public float Time
	  {
		get { return time; }
		set { time = value; }
	  }

	  private float value1;

	  public float Value1
	  {
		get { return value1; }
		set { value1 = value; }
	  }

	  private float value2;

	  public float Value2
	  {
		get { return value2; }
		set { value2 = value; }
	  }

	  public override int GetHashCode()
	  {
		return base.GetHashCode();
	  }

	  public override bool Equals(object x)
	  {
		IrregularTimePoint p = (IrregularTimePoint)x;
		if (base.Equals(x))
		{
		    return (p.IntervalSchedule == intervalSchedule && p.Time == time && p.Value1 == value1 && p.Value2 == value2);
		}
		return false;
	  }

	  public override bool HasProperty(ModelCode property)
	  {
		switch (property)
		{
		    case ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE:
		    case ModelCode.IRREGULARTIMEPOINT_TIME:
		    case ModelCode.IRREGULARTIMEPOINT_VALUE1:
		    case ModelCode.IRREGULARTIMEPOINT_VALUE2:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }

	  public override void GetProperty(Property property)
	  {
		switch(property.Id)
		{
		    case ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE:
			  property.SetValue(intervalSchedule);
			  break;
		    case ModelCode.IRREGULARTIMEPOINT_TIME:
			  property.SetValue(time);
			  break;
		    case ModelCode.IRREGULARTIMEPOINT_VALUE1:
			  property.SetValue(value1);
			  break;
		    case ModelCode.IRREGULARTIMEPOINT_VALUE2:
			  property.SetValue(value2);
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
		    case ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE:
			  intervalSchedule = property.AsReference();
			  break;
		    case ModelCode.IRREGULARTIMEPOINT_TIME:
			  time = property.AsFloat();
			  break;
		    case ModelCode.IRREGULARTIMEPOINT_VALUE1:
			  value1 = property.AsFloat();
			  break;
		    case ModelCode.IRREGULARTIMEPOINT_VALUE2:
			  value2 = property.AsFloat();
			  break;
		    default:
			  base.SetProperty(property);
			  break;
		}
	  }

	  public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
	  {
		if(intervalSchedule != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
		{
		    references[ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE] = new List<long>();
		    references[ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE].Add(intervalSchedule);
		}
	  }
    }
}
