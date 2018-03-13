using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class CurveData : IdentifiedObject
    {
	  public CurveData(long globalId) : base(globalId)
	  {
	  }

	  private long curve;

	  public long Curve
	  {
		get { return curve; }
		set { curve = value; }
	  }


	  private float xValue;

	  public float XValue
	  {
		get { return xValue; }
		set { xValue = value; }
	  }

	  private float y1Value;

	  public float Y1Value
	  {
		get { return y1Value; }
		set { y1Value = value; }
	  }

	  private float y2Value;

	  public float Y2Value
	  {
		get { return y2Value; }
		set { y2Value = value; }
	  }

	  private float y3Value;

	  public float Y3Value
	  {
		get { return y3Value; }
		set { y3Value = value; }
	  }

	  public override int GetHashCode()
	  {
		return base.GetHashCode();
	  }

	  public override bool Equals(object x)
	  {
		CurveData cd = (CurveData)x;
		if(base.Equals(x))
		{
		    return (cd.XValue == xValue && cd.Y1Value == y1Value && cd.Y2Value == y2Value && cd.y3Value == y3Value && cd.Curve == curve);
		}
		return false;
	  }

	  public override bool HasProperty(ModelCode property)
	  {
		switch(property)
		{
		    case ModelCode.CURVEDATA_XVALUE:
		    case ModelCode.CURVEDATA_Y1VALUE:
		    case ModelCode.CURVEDATA_Y2VALUE:
		    case ModelCode.CURVEDATA_Y3VALUE:
		    case ModelCode.CURVEDATA_CURVE:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }

	  public override void GetProperty(Property property)
	  {
		switch (property.Id)
		{
		    case ModelCode.CURVEDATA_XVALUE:
			  property.SetValue(xValue);
			  break;
		    case ModelCode.CURVEDATA_Y1VALUE:
			  property.SetValue(y1Value);
			  break;
		    case ModelCode.CURVEDATA_Y2VALUE:
			  property.SetValue(y2Value);
			  break;
		    case ModelCode.CURVEDATA_Y3VALUE:
			  property.SetValue(y3Value);
			  break;
		    case ModelCode.CURVEDATA_CURVE:
			  property.SetValue(curve);
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
		    case ModelCode.CURVEDATA_CURVE:
			  curve = property.AsReference();
			  break;
		    case ModelCode.CURVEDATA_XVALUE:
			  xValue = property.AsFloat();
			  break;
		    case ModelCode.CURVEDATA_Y1VALUE:
			  y1Value = property.AsFloat();
			  break;
		    case ModelCode.CURVEDATA_Y2VALUE:
			  y2Value = property.AsFloat();
			  break;
		    case ModelCode.CURVEDATA_Y3VALUE:
			  y3Value = property.AsFloat();
			  break;
		    default:
			  base.SetProperty(property);
			  break;
		}
	  }

	  public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
	  {
		if(curve!= 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
		{
		    references[ModelCode.CURVEDATA_CURVE] = new List<long>();
		    references[ModelCode.CURVEDATA_CURVE].Add(curve);
		    return;
		}

		base.GetReferences(references, refType);

	  }
    }
}
