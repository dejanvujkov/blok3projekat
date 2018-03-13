namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    using System;
    using FTN.Common;

    /// <summary>
    /// PowerTransformerConverter has methods for populating
    /// ResourceDescription objects using PowerTransformerCIMProfile_Labs objects.
    /// </summary>
    public static class PowerTransformerConverter
    {

	  #region Populate ResourceDescription
	  public static void PopulateIdentifiedObjectProperties(IdentifiedObject cimIdentifiedObject, ResourceDescription rd)
	  {
		if(cimIdentifiedObject != null && rd != null)
		{
		    if (cimIdentifiedObject.MRIDHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cimIdentifiedObject.MRID));
		    }
		    if (cimIdentifiedObject.NameHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cimIdentifiedObject.Name));
		    }
		    if (cimIdentifiedObject.AliasNameHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cimIdentifiedObject.AliasName));
		    }
		}
	  }

	  public static void PopulateCurveDataProperties(CurveData curveData, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
	  {
		if(curveData != null && rd != null)
		{
		    PowerTransformerConverter.PopulateIdentifiedObjectProperties(curveData, rd);
		    if (curveData.XvalueHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVEDATA_XVALUE));
		    }
		    if (curveData.CurveHasValue)
		    {
			  long gid = importHelper.GetMappedGID(curveData.Curve.ID);
			  if(gid < 0)
			  {
				report.Report.Append("WARNING: Convert ").Append(curveData.GetType().ToString()).Append(" rdfID = \"").Append(curveData.ID);
				report.Report.Append("\" - Failed to set reference to Curve: rdfID \"").Append(curveData.Curve.ID).AppendLine(" \" is not mapped to GID!");
			  }
			  else
			  {
				rd.AddProperty(new Property(ModelCode.CURVEDATA_CURVE, gid));
			  }
		    }
		    if (curveData.Y1valueHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVEDATA_Y1VALUE, curveData.Y1value));
		    }
		    if (curveData.Y2valueHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVEDATA_Y2VALUE, curveData.Y2value));
		    }
		    if (curveData.Y3valueHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVEDATA_Y3VALUE, curveData.Y3value));
		    }
		}
	  }

	  public static void PopulateCurveProperties(Curve curve, ResourceDescription rd)
	  {
		PowerTransformerConverter.PopulateIdentifiedObjectProperties(curve, rd);
		if(curve != null && rd != null)
		{
		    if (curve.CurveStyleHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_STYLE, (short)GetDmsCurveStyle(curve.CurveStyle)));
		    }
		    if (curve.XMultiplierHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_XMULTPLIER, (short)GetDMSUnitMultiplier(curve.XMultiplier)));
		    }
		    if (curve.XUnitHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_XUNIT, (short)GetDMSUNitSymbol(curve.XUnit)));
		    }
		    if (curve.Y1MultiplierHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_Y1MULTIPLIER, (short)GetDMSUnitMultiplier(curve.Y1Multiplier)));
		    }
		    if (curve.Y1UnitHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_Y1UNIT, (short)GetDMSUNitSymbol(curve.Y1Unit)));
		    }
		    if (curve.Y2MultiplierHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_Y2MULTIPLIER, (short)GetDMSUnitMultiplier(curve.Y2Multiplier)));
		    }
		    if (curve.Y2UnitHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_Y2UNIT, (short)GetDMSUNitSymbol(curve.Y2Unit)));
		    }
		    if (curve.Y3MultiplierHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_Y3MULTIPLIER, (short)GetDMSUnitMultiplier(curve.Y3Multiplier)));
		    }
		    if (curve.Y3UnitHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.CURVE_Y3UNIT, (short)GetDMSUNitSymbol(curve.Y3Unit)));
		    }
		}
	  }

	  public static void PopulatePowerSystemResourceProperties(PowerSystemResource powerSystemResource, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
	  {
		if(powerSystemResource != null && rd != null)
		{
		    PowerTransformerConverter.PopulateIdentifiedObjectProperties(powerSystemResource, rd);

		    if (powerSystemResource.OutageScheduleHasValue)
		    {
			  long gid = importHelper.GetMappedGID(powerSystemResource.OutageSchedule.ID);
			  if (gid < 0)
			  {
				report.Report.Append("WARNING: Convert ").Append(powerSystemResource.GetType().ToString()).Append(" rdfID = \"").Append(powerSystemResource.ID);
				report.Report.Append("\" - Failed to set reference to OutageSchedule: rdfID \"").Append(powerSystemResource.OutageSchedule.ID).AppendLine(" \" is not mapped to GID!");
			  }
			  else
			  {
				rd.AddProperty(new Property(ModelCode.PSR_OUTAGESCHEDULE, gid));
			  }
		    }
		}
		
	  }

	  public static void PopulateEquipmentProperties(FTN.Equipment cimEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
	  {
		if(cimEquipment != null && rd != null)
		{
		    PowerTransformerConverter.PopulatePowerSystemResourceProperties(cimEquipment, rd, importHelper, report);

		    if (cimEquipment.AggregateHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cimEquipment.Aggregate));
		    }
		    if (cimEquipment.NormallyInServiceHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.EQUIPEMNT_NORMINSERVICE, cimEquipment.NormallyInService));
		    }
		}
	  }

	  public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cimConductingEquipment, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
	  {
		if(cimConductingEquipment != null && rd != null)
		{
		    PowerTransformerConverter.PopulateEquipmentProperties(cimConductingEquipment, rd, importHelper, report);
		}
	  }

	  public static void PopulateSwitchProperties(Switch s, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
	  { 
		if(s != null && rd != null)
		{
		    PowerTransformerConverter.PopulateConductingEquipmentProperties(s, rd, importHelper, report);
		    if (s.NormalOpenHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.SWITCH_NORMOPEN, s.NormalOpen));
		    }
		    if (s.RatedCurrentHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.SWITCH_RATEDCURRENT, s.RatedCurrent));
		    }
		    if (s.RetainedHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.SWITCH_RETAINED, s.Retained));
		    }
		    if (s.SwitchOnCountHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.SWITCH_ONCOUNT, s.SwitchOnCount));
		    }
		    if (s.SwitchOnDateHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.SWITCH_ONDATE, s.SwitchOnDate));
		    }
		}
	  }

	  public static void PopulateProtectedSwitchProperties(ProtectedSwitch ps, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
	  {
		if (ps != null && rd != null)
		{
		    PowerTransformerConverter.PopulateSwitchProperties(ps, rd, importHelper, report);
		}
	  }

	  public static void PopulateDisconnectorProperties(Disconnector d, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
	  {
		if(d != null && rd != null)
		{
		    PowerTransformerConverter.PopulateSwitchProperties(d, rd, importHelper, report);
		}
	  }

	  public static void PopulateBasicIntervalScheduleProperties(BasicIntervalSchedule b, ResourceDescription rd)
	  {
		if(b != null && rd != null)
		{
		    PowerTransformerConverter.PopulateIdentifiedObjectProperties(b, rd);

		    if (b.StartTimeHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.BINTERVALSCHEDULE_STARTTIME, b.StartTime));
		    }
		    if (b.Value1MultiplierHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.BINTERVALSCHEDULE_V1MULTIPLIER, (short)GetDMSUnitMultiplier(b.Value1Multiplier)));
		    }
		    if (b.Value1UnitHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.BINTERVALSCHEDULE_V1UNIT, (short)GetDMSUNitSymbol(b.Value1Unit)));
		    }
		    if (b.Value2MultiplierHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.BINTERVALSCHEDULE_V2MULTIPLIER, (short)GetDMSUnitMultiplier(b.Value2Multiplier)));
		    }
		    if (b.Value2UnitHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.BINTERVALSCHEDULE_V2UNIT, (short)GetDMSUNitSymbol(b.Value2Unit)));
		    }
		}
	  }

	  public static void PopulateIrregularIntervlScheduleProperties(IrregularIntervalSchedule i, ResourceDescription rd)
	  {
		if (i != null && rd != null)
		{
		    PowerTransformerConverter.PopulateBasicIntervalScheduleProperties(i, rd);
		}
	  }

	  public static void PopulateRegularIntervalScheduleProperties(RegularIntervalSchedule r, ResourceDescription rd)
	  {
		if(r != null && rd != null)
		{
		    PowerTransformerConverter.PopulateBasicIntervalScheduleProperties(r, rd);

		    if (r.EndTimeHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.REGULARINTERVALSCHEDULE_ENDTIME, r.EndTime));
		    }

		    if (r.TimeStepHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.REGULARINTERVALSCHEDULE_TIMESTEP, r.TimeStep));
		    }
		}
	  }

	  public static void PopulateRegularTimePointProperties(RegularTimePoint r, ResourceDescription rd, ImportHelper i, TransformAndLoadReport report)
	  {
		if(r != null && rd != null)
		{
		    PowerTransformerConverter.PopulateIdentifiedObjectProperties(r, rd);

		    if (r.SequenceNumberHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_SEQUENCENO, r.SequenceNumber));
		    }
		    if (r.Value1HasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_VALUE1, r.Value1));
		    }
		    if (r.Value2HasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_VALUE2, r.Value2));
		    }
		    if (r.IntervalScheduleHasValue)
		    {
			  long gid = i.GetMappedGID(r.IntervalSchedule.ID);
			  if(gid < 0)
			  {
				report.Report.Append("WARNING: Convert ").Append(r.GetType().ToString()).Append(" rdfID = \"").Append(r.ID);
				report.Report.Append("\" - Failed to set reference to IntervalSchedule: rdfID \"").Append(r.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
			  }
			  else 
				rd.AddProperty(new Property(ModelCode.REGULARTIMEPOINT_INTERVALSCHEDULE, gid));
		    }
		}
	  }

	  public static void PopulateIrregularTimePointProperties(IrregularTimePoint point, ResourceDescription rd, ImportHelper i, TransformAndLoadReport report)
	  {
		if(point != null && rd != null)
		{
		    PowerTransformerConverter.PopulateIdentifiedObjectProperties(point, rd);
		    if (point.TimeHasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.IRREGULARTIMEPOINT_TIME, point.Time));
		    }
		    if (point.Value1HasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.IRREGULARTIMEPOINT_VALUE1, point.Value1));
		    }
		    if (point.Value2HasValue)
		    {
			  rd.AddProperty(new Property(ModelCode.IRREGULARTIMEPOINT_VALUE2, point.Value2));
		    }
		    if (point.IntervalScheduleHasValue)
		    {
			  long gid = i.GetMappedGID(point.IntervalSchedule.ID);
			  if (gid < 0)
			  {
				report.Report.Append("WARNING: Convert ").Append(point.GetType().ToString()).Append(" rdfID = \"").Append(point.ID);
				report.Report.Append("\" - Failed to set reference to IntervalSchedule: rdfID \"").Append(point.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
			  }
			  else
				rd.AddProperty(new Property(ModelCode.IRREGULARTIMEPOINT_INTERVALSCHEDULE, gid));
		    }
		}
	  }

	  public static void PopulateOutageScheduleProperties(OutageSchedule o, ResourceDescription rd, ImportHelper i, TransformAndLoadReport report)
	  {
		if(o != null && rd != null)
		{
		    PowerTransformerConverter.PopulateIrregularIntervlScheduleProperties(o, rd);
		}
	  }
	  #endregion Populate ResourceDescription

	  #region Enums convert
	  public static CurveStyle GetDmsCurveStyle(FTN.CurveStyle curveStyle)
	  {
		switch (curveStyle)
		{
		    case FTN.CurveStyle.constantYValue:
			  return CurveStyle.constantYValue;
		    case FTN.CurveStyle.formula:
			  return CurveStyle.formula;
		    case FTN.CurveStyle.rampYValue:
			  return CurveStyle.rampYValue;
		    default:
			  return CurveStyle.straightLineYValues;
		}
	  }

	  public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier unitMultiplier)
	  {
		switch (unitMultiplier)
		{
		    case FTN.UnitMultiplier.c:
			  return UnitMultiplier.c;
		    case FTN.UnitMultiplier.d:
			  return UnitMultiplier.d;
		    case FTN.UnitMultiplier.G:
			  return UnitMultiplier.G;
		    case FTN.UnitMultiplier.k:
			  return UnitMultiplier.k;
		    case FTN.UnitMultiplier.m:
			  return UnitMultiplier.m;
		    case FTN.UnitMultiplier.M:
			  return UnitMultiplier.M;
		    case FTN.UnitMultiplier.micro:
			  return UnitMultiplier.micro;
		    case FTN.UnitMultiplier.n:
			  return UnitMultiplier.n;
		    case FTN.UnitMultiplier.none:
			  return UnitMultiplier.none;
		    case FTN.UnitMultiplier.p:
			  return UnitMultiplier.p;
		    default:
			  return UnitMultiplier.T;
		}
	  }

	  public static UnitSymbol GetDMSUNitSymbol(FTN.UnitSymbol unitSymbol)
	  {
		switch (unitSymbol)
		{
		    case FTN.UnitSymbol.A:
			  return UnitSymbol.A;
		    case FTN.UnitSymbol.deg:
			  return UnitSymbol.deg;
		    case FTN.UnitSymbol.degC:
			  return UnitSymbol.degC;
		    case FTN.UnitSymbol.F:
			  return UnitSymbol.g;
		    case FTN.UnitSymbol.h:
			  return UnitSymbol.h;
		    case FTN.UnitSymbol.H:
			  return UnitSymbol.H;
		    case FTN.UnitSymbol.Hz:
			  return UnitSymbol.Hz;
		    case FTN.UnitSymbol.J:
			  return UnitSymbol.J;
		    case FTN.UnitSymbol.m:
			  return UnitSymbol.m;
		    case FTN.UnitSymbol.m2:
			  return UnitSymbol.m2;
		    case FTN.UnitSymbol.m3:
			  return UnitSymbol.m3;
		    case FTN.UnitSymbol.min:
			  return UnitSymbol.min;
		    case FTN.UnitSymbol.N:
			  return UnitSymbol.N;
		    case FTN.UnitSymbol.none:
			  return UnitSymbol.none;
		    case FTN.UnitSymbol.ohm:
			  return UnitSymbol.ohm;
		    case FTN.UnitSymbol.Pa:
			  return UnitSymbol.Pa;
		    case FTN.UnitSymbol.rad:
			  return UnitSymbol.rad;
		    case FTN.UnitSymbol.s:
			  return UnitSymbol.s;
		    case FTN.UnitSymbol.S:
			  return UnitSymbol.S;
		    case FTN.UnitSymbol.V:
			  return UnitSymbol.V;
		    case FTN.UnitSymbol.VA:
			  return UnitSymbol.VA;
		    case FTN.UnitSymbol.VAh:
			  return UnitSymbol.VAh;
		    case FTN.UnitSymbol.VAr:
			  return UnitSymbol.VAr;
		    case FTN.UnitSymbol.VArh:
			  return UnitSymbol.VArh;
		    case FTN.UnitSymbol.W:
			  return UnitSymbol.W;
		    default:
			  return UnitSymbol.Wh;
		}
	  }
	  #endregion Enums convert
    }
}
