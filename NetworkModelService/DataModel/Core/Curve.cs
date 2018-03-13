using FTN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class Curve : IdentifiedObject
    {
	  public Curve(long globalId) : base(globalId)
	  {
	  }

	  private List<long> curveDatas = new List<long>();

	  private CurveStyle curveStyle;

	  public CurveStyle CurveStyle
	  {
		get { return curveStyle; }
		set { curveStyle = value; }
	  }

	  private UnitMultiplier xMultiplier;

	  public UnitMultiplier XMultiplier
	  {
		get { return xMultiplier; }
		set { xMultiplier = value; }
	  }

	  private UnitSymbol xUnit;

	  public UnitSymbol XUnit
	  {
		get { return xUnit; }
		set { xUnit = value; }
	  }

	  private UnitMultiplier y1Multiplier;

	  public UnitMultiplier Y1Multiplier
	  {
		get { return y1Multiplier; }
		set { y1Multiplier = value; }
	  }

	  private UnitSymbol y1Unit;

	  public UnitSymbol Y1Unit
	  {
		get { return y1Unit; }
		set { y1Unit = value; }
	  }

	  private UnitMultiplier y2Multiplier;

	  public UnitMultiplier Y2Multiplier
	  {
		get { return y2Multiplier; }
		set { y2Multiplier = value; }
	  }

	  private UnitSymbol y2Unit;

	  public UnitSymbol Y2Unit
	  {
		get { return y2Unit; }
		set { y2Unit = value; }
	  }

	  private UnitMultiplier y3Multiplier;

	  public UnitMultiplier Y3Multiplier
	  {
		get { return y3Multiplier; }
		set { y3Multiplier = value; }
	  }

	  private UnitSymbol y3Unit;

	  public UnitSymbol Y3Unit
	  {
		get { return y3Unit; }
		set { y3Unit = value; }
	  }

	  public override int GetHashCode()
	  {
		return base.GetHashCode();
	  }

	  public override bool Equals(object x)
	  {
		Curve c = (Curve)x;
		if (base.Equals(x))
		{
		    return (c.curveDatas == curveDatas && c.CurveStyle == curveStyle && c.XMultiplier == xMultiplier && c.XUnit == xUnit && c.Y1Multiplier == y1Multiplier && c.Y1Unit == y1Unit && c.Y2Multiplier == y2Multiplier && c.Y2Unit == y2Unit && c.Y3Multiplier == y3Multiplier && c.Y3Unit == y3Unit);
		}

		return false;
	  }
	  public override bool IsReferenced => curveDatas.Count > 0 || base.IsReferenced;

	  public override bool HasProperty(ModelCode property)
	  {
		switch(property)
		{
		    case ModelCode.CURVE_CURVEDATAS:
		    case ModelCode.CURVE_STYLE:
		    case ModelCode.CURVE_XMULTPLIER:
		    case ModelCode.CURVE_XUNIT:
		    case ModelCode.CURVE_Y1MULTIPLIER:
		    case ModelCode.CURVE_Y1UNIT:
		    case ModelCode.CURVE_Y2MULTIPLIER:
		    case ModelCode.CURVE_Y2UNIT:
		    case ModelCode.CURVE_Y3MULTIPLIER:
		    case ModelCode.CURVE_Y3UNIT:
			  return true;
		    default:
			  return base.HasProperty(property);
		}
	  }

	  public override void GetProperty(Property property)
	  {
		switch(property.Id)
		{
		    case ModelCode.CURVE_CURVEDATAS:
			  property.SetValue(curveDatas);
			  break;
		    case ModelCode.CURVE_STYLE:
			  property.SetValue((short)curveStyle);
			  break;
		    case ModelCode.CURVE_XMULTPLIER:
			  property.SetValue((short)xMultiplier);
			  break;
		    case ModelCode.CURVE_XUNIT:
			  property.SetValue((short)xUnit);
			  break;
		    case ModelCode.CURVE_Y1MULTIPLIER:
			  property.SetValue((short)y1Multiplier);
			  break;
		    case ModelCode.CURVE_Y1UNIT:
			  property.SetValue((short)y1Unit);
			  break;
		    case ModelCode.CURVE_Y2MULTIPLIER:
			  property.SetValue((short)y2Multiplier);
			  break;
		    case ModelCode.CURVE_Y2UNIT:
			  property.SetValue((short)y2Unit);
			  break;
		    case ModelCode.CURVE_Y3MULTIPLIER:
			  property.SetValue((short)y3Multiplier);
			  break;
		    case ModelCode.CURVE_Y3UNIT:
			  property.SetValue((short)y3Unit);
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
		    case ModelCode.CURVE_STYLE:
			  curveStyle = (CurveStyle)property.AsEnum();
			  break;
		    case ModelCode.CURVE_XMULTPLIER:
			  xMultiplier = (UnitMultiplier)property.AsEnum();
			  break;
		    case ModelCode.CURVE_XUNIT:
			  xUnit = (UnitSymbol)property.AsEnum();
			  break;
		    case ModelCode.CURVE_Y1MULTIPLIER:
			  y1Multiplier = (UnitMultiplier)property.AsEnum();
			  break;
		    case ModelCode.CURVE_Y1UNIT:
			  y1Unit = (UnitSymbol)property.AsEnum();
			  break;
		    case ModelCode.CURVE_Y2MULTIPLIER:
			  y2Multiplier = (UnitMultiplier)property.AsEnum();
			  break;
		    case ModelCode.CURVE_Y2UNIT:
			  y2Unit = (UnitSymbol)property.AsEnum();
			  break;
		    case ModelCode.CURVE_Y3MULTIPLIER:
			  y3Multiplier = (UnitMultiplier)property.AsEnum();
			  break;
		    case ModelCode.CURVE_Y3UNIT:
			  y3Unit = (UnitSymbol)property.AsEnum();
			  break;
		    default:
			  base.SetProperty(property);
			  break;
		}
	  }

	  public override void AddReference(ModelCode referenceId, long globalId)
	  {
		switch (referenceId)
		{
		    case ModelCode.CURVEDATA_CURVE:
			  curveDatas.Add(globalId);
			  break;
		    default:
			  base.AddReference(referenceId, globalId);
			  break;
		}
	  }

	  public override void RemoveReference(ModelCode referenceId, long globalId)
	  {
		switch(referenceId)
		{
		    case ModelCode.CURVEDATA_CURVE:
			  if (!curveDatas.Remove(globalId))
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
		if (curveDatas.Count > 0 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
		{
		    references[ModelCode.CURVE_CURVEDATAS] = new List<long>();
		    references[ModelCode.CURVE_CURVEDATAS] = curveDatas.GetRange(0, curveDatas.Count);
		    return;
		}

		base.GetReferences(references, refType);
	  }
    }
}
