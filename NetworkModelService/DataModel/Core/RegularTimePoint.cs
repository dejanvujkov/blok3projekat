using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class RegularTimePoint : IdentifiedObject
    {
	  public RegularTimePoint(long globalId) : base(globalId)
	  {
	  }

	  private long intervalSchedule;

	  public long IntervalSchedule
	  {
		get { return intervalSchedule; }
		set { intervalSchedule = value; }
	  }

	  private int seqNumber;

	  public int SeqNumber
	  {
		get { return seqNumber; }
		set { seqNumber = value; }
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

	  public override bool HasProperty(ModelCode property)
	  {
		switch(property)
		{
		    case ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE:
		    case ModelCode.REGULARTIMEPOINT_SEQUENCENO:
		    case ModelCode.REGULARTIMEPOINT_VALUE1:
		    case ModelCode.REGULARTIMEPOINT_VALUE2:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }
	  public override void GetProperty(Property property)
	  {
		switch(property.Id)
		{
		    case ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE:
			  property.SetValue(intervalSchedule);
			  break;
		    case ModelCode.REGULARTIMEPOINT_SEQUENCENO:
			  property.SetValue(seqNumber);
			  break;
		    case ModelCode.REGULARTIMEPOINT_VALUE1:
			  property.SetValue(value1);
			  break;
		    case ModelCode.REGULARTIMEPOINT_VALUE2:
			  property.SetValue(value2);
			  break;
		    default:
			  base.GetProperty(property);
			  break;
		}
	  }

	  public override bool Equals(object x)
	  {
		RegularTimePoint r = (RegularTimePoint)x;
		if (base.Equals(x))
		{
		    return (r.IntervalSchedule == intervalSchedule && r.SeqNumber == seqNumber && r.Value1 == value1 && r.Value2 == value2);
		}

		return false;
	  }

	  public override void SetProperty(Property property)
	  {
		switch(property.Id)
		{
		    case ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE:
			  intervalSchedule = property.AsReference();
			  break;
		    case ModelCode.REGULARTIMEPOINT_SEQUENCENO:
			  seqNumber = property.AsInt();
			  break;
		    case ModelCode.REGULARTIMEPOINT_VALUE1:
			  value1 = property.AsFloat();
			  break;
		    case ModelCode.REGULARTIMEPOINT_VALUE2:
			  value2 = property.AsFloat();
			  break;
		    default:
			  base.SetProperty(property);
			  break;
		}
	  }
        

	  public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
	  {

		if (intervalSchedule != 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
		{
		    references[ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE] = new List<long>();
		    references[ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE].Add(intervalSchedule);
		    return;
		}

		base.GetReferences(references, refType);
	  }
    }
}
