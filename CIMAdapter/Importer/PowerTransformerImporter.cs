using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    /// <summary>
    /// PowerTransformerImporter
    /// </summary>
    public class PowerTransformerImporter
    {
	  /// <summary> Singleton </summary>
	  private static PowerTransformerImporter ptImporter = null;
	  private static object singletoneLock = new object();

	  private ConcreteModel concreteModel;
	  private Delta delta;
	  private ImportHelper importHelper;
	  private TransformAndLoadReport report;


	  #region Properties
	  public static PowerTransformerImporter Instance
	  {
		get
		{
		    if (ptImporter == null)
		    {
			  lock (singletoneLock)
			  {
				if (ptImporter == null)
				{
				    ptImporter = new PowerTransformerImporter();
				    ptImporter.Reset();
				}
			  }
		    }
		    return ptImporter;
		}
	  }

	  public Delta NMSDelta
	  {
		get
		{
		    return delta;
		}
	  }
	  #endregion Properties


	  public void Reset()
	  {
		concreteModel = null;
		delta = new Delta();
		importHelper = new ImportHelper();
		report = null;
	  }

	  public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
	  {
		LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
		report = new TransformAndLoadReport();
		concreteModel = cimConcreteModel;
		delta.ClearDeltaOperations();

		if ((concreteModel != null) && (concreteModel.ModelMap != null))
		{
		    try
		    {
			  // convert into DMS elements
			  ConvertModelAndPopulateDelta();
		    }
		    catch (Exception ex)
		    {
			  string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
			  LogManager.Log(message);
			  report.Report.AppendLine(ex.Message);
			  report.Success = false;
		    }
		}
		LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
		return report;
	  }

	  /// <summary>
	  /// Method performs conversion of network elements from CIM based concrete model into DMS model.
	  /// </summary>
	  private void ConvertModelAndPopulateDelta()
	  {
		LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

		//// import all concrete model types (DMSType enum)
		ImportCurve();
		ImportCurveData();
		ImportOutageSchedule();
		ImportProtectedSwitch();
		ImportDisconnector();
		ImportIrregularTimePoint();
		ImportRegularIntervalSchedule();
		ImportRegularTimePoint();
		LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
	  }

	  private void ImportRegularIntervalSchedule()
	  {
		SortedDictionary<string, object> ps = concreteModel.GetAllObjectsOfType("FTN.RegularIntervalSchedule");

		foreach (var v in ps)
		{
		    RegularIntervalSchedule s = v.Value as RegularIntervalSchedule;

		    ResourceDescription rd = CreateRegularIntervalScheduleResourceDescription(s);
		    if (rd != null)
		    {
			  delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
			  report.Report.Append("REGULARINTERVALSCHEDULE ID = ").Append(s.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		    }
		    else
		    {
			  report.Report.Append("REGULARINTERVALSCHEDULE ID = ").Append(s.ID).AppendLine(" FAILED to be converted");
		    }
		}
		report.Report.AppendLine();
	  }

	  private ResourceDescription CreateRegularIntervalScheduleResourceDescription(RegularIntervalSchedule s)
	  {
		ResourceDescription rd = null;
		if (s != null)
		{
		    long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULARINTERVALSCHEDULE, importHelper.CheckOutIndexForDMSType(DMSType.REGULARINTERVALSCHEDULE));
		    rd = new ResourceDescription(gid);
		    importHelper.DefineIDMapping(s.ID, gid);
		    PowerTransformerConverter.PopulateRegularIntervalScheduleProperties(s, rd);
		}
		return rd;
	  }

	  private void ImportCurveData()
	  {
		SortedDictionary<string, object> ps = concreteModel.GetAllObjectsOfType("FTN.CurveData");

		foreach (var v in ps)
		{
		    CurveData s = v.Value as CurveData;

		    ResourceDescription rd = CreateCurveDataResourceDescription(s);
		    if (rd != null)
		    {
			  delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
			  report.Report.Append("CURVEDATA ID = ").Append(s.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		    }
		    else
		    {
			  report.Report.Append("CURVEDATA ID = ").Append(s.ID).AppendLine(" FAILED to be converted");
		    }
		}
		report.Report.AppendLine();
	  }

	  private ResourceDescription CreateCurveDataResourceDescription(CurveData s)
	  {
		ResourceDescription rd = null;
		if (s != null)
		{
		    long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CURVEDATA, importHelper.CheckOutIndexForDMSType(DMSType.CURVEDATA));
		    rd = new ResourceDescription(gid);
		    importHelper.DefineIDMapping(s.ID, gid);
		    PowerTransformerConverter.PopulateCurveDataProperties(s, rd, importHelper, report);
		}
		return rd;
	  }

	  private void ImportOutageSchedule()
	  {
		SortedDictionary<string, object> ps = concreteModel.GetAllObjectsOfType("FTN.OutageSchedule");

		foreach (var v in ps)
		{
		    OutageSchedule s = v.Value as OutageSchedule;

		    ResourceDescription rd = CreateOutageScheduleResourceDescription(s);
		    if (rd != null)
		    {
			  delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
			  report.Report.Append("OUTAGESCHEDULE ID = ").Append(s.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		    }
		    else
		    {
			  report.Report.Append("OUTAGESCHEDULE ID = ").Append(s.ID).AppendLine(" FAILED to be converted");
		    }
		}
		report.Report.AppendLine();
	  }

	  private ResourceDescription CreateOutageScheduleResourceDescription(OutageSchedule s)
	  {
		ResourceDescription rd = null;
		if (s != null)
		{
		    long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.OUTAGESCHEDULE, importHelper.CheckOutIndexForDMSType(DMSType.OUTAGESCHEDULE));
		    rd = new ResourceDescription(gid);
		    importHelper.DefineIDMapping(s.ID, gid);
		    PowerTransformerConverter.PopulateOutageScheduleProperties(s, rd, importHelper, report);
		}
		return rd;
	  }

	  private void ImportIrregularTimePoint()
	  {
		SortedDictionary<string, object> ps = concreteModel.GetAllObjectsOfType("FTN.IrregularTimePoint");

		foreach (var v in ps)
		{
		    IrregularTimePoint s = v.Value as IrregularTimePoint;

		    ResourceDescription rd = CreateIrregularTimePointResourceDescription(s);
		    if (rd != null)
		    {
			  delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
			  report.Report.Append("IRREGULARTIMEPOINT ID = ").Append(s.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		    }
		    else
		    {
			  report.Report.Append("IRREGULARTIEPOINT ID = ").Append(s.ID).AppendLine(" FAILED to be converted");
		    }
		}
		report.Report.AppendLine();
	  }

	  private ResourceDescription CreateIrregularTimePointResourceDescription(IrregularTimePoint s)
	  {
		ResourceDescription rd = null;
		if (s != null)
		{
		    long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.IRREGULARTIMEPOINT, importHelper.CheckOutIndexForDMSType(DMSType.IRREGULARTIMEPOINT));
		    rd = new ResourceDescription(gid);
		    importHelper.DefineIDMapping(s.ID, gid);
		    PowerTransformerConverter.PopulateIrregularTimePointProperties(s, rd, importHelper, report);
		}
		return rd;
	  }

	  private void ImportRegularTimePoint()
	  {
		SortedDictionary<string, object> ps = concreteModel.GetAllObjectsOfType("FTN.RegularTimePoint");

		foreach (var v in ps)
		{
		    RegularTimePoint s = v.Value as RegularTimePoint;

		    ResourceDescription rd = CreateRegularTimePointResourceDescription(s);
		    if (rd != null)
		    {
			  delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
			  report.Report.Append("REGULARTIMEPOINT ID = ").Append(s.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		    }
		    else
		    {
			  report.Report.Append("REGULARTIEPOINT ID = ").Append(s.ID).AppendLine(" FAILED to be converted");
		    }
		}
		report.Report.AppendLine();
	  }

	  private ResourceDescription CreateRegularTimePointResourceDescription(RegularTimePoint s)
	  {
		ResourceDescription rd = null;
		if (s != null)
		{
		    long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULARTIMEPOINT, importHelper.CheckOutIndexForDMSType(DMSType.REGULARTIMEPOINT));
		    rd = new ResourceDescription(gid);
		    importHelper.DefineIDMapping(s.ID, gid);
		    PowerTransformerConverter.PopulateRegularTimePointProperties(s, rd, importHelper, report);
		}
		return rd;
	  }

	  private void ImportCurve()
	  {
		SortedDictionary<string, object> ps = concreteModel.GetAllObjectsOfType("FTN.Curve");

		foreach (var v in ps)
		{
		    Curve s = v.Value as Curve;

		    ResourceDescription rd = CreateCurveResourceDescription(s);
		    if (rd != null)
		    {
			  delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
			  report.Report.Append("CURVE ID = ").Append(s.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		    }
		    else
		    {
			  report.Report.Append("CURVE ID = ").Append(s.ID).AppendLine(" FAILED to be converted");
		    }
		}
		report.Report.AppendLine();
	  }

	  private ResourceDescription CreateCurveResourceDescription(Curve s)
	  {
		ResourceDescription rd = null;
		if (s != null)
		{
		    long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.CURVE, importHelper.CheckOutIndexForDMSType(DMSType.CURVE));
		    rd = new ResourceDescription(gid);
		    importHelper.DefineIDMapping(s.ID, gid);
		    PowerTransformerConverter.PopulateCurveProperties(s, rd);
		}
		return rd;
	  }

	  private void ImportDisconnector()
	  {
		SortedDictionary<string, object> ps = concreteModel.GetAllObjectsOfType("FTN.Disconnector");

		foreach (var v in ps)
		{
		    Disconnector s = v.Value as Disconnector;

		    ResourceDescription rd = CreateDisconnectorResourceDescription(s);
		    if (rd != null)
		    {
			  delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
			  report.Report.Append("DISCONNECTOR ID = ").Append(s.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		    }
		    else
		    {
			  report.Report.Append("DISCONNECTOR ID = ").Append(s.ID).AppendLine(" FAILED to be converted");

		    }
		}
		report.Report.AppendLine();
	  }

	  private ResourceDescription CreateDisconnectorResourceDescription(Disconnector s)
	  {
		ResourceDescription rd = null;
		if (s != null)
		{
		    long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DISCONNECTOR, importHelper.CheckOutIndexForDMSType(DMSType.DISCONNECTOR));
		    rd = new ResourceDescription(gid);
		    importHelper.DefineIDMapping(s.ID, gid);
		    PowerTransformerConverter.PopulateDisconnectorProperties(s, rd, importHelper, report);
		}
		return rd;
	  }

	  private void ImportProtectedSwitch()
	  {
		SortedDictionary<string, object> ps = concreteModel.GetAllObjectsOfType("FTN.ProtectedSwitch");

		foreach (var v in ps)
		{
		    ProtectedSwitch s = v.Value as ProtectedSwitch;

		    ResourceDescription rd = CreateProtectedSwitchResourceDescription(s);
		    if (rd != null)
		    {
			  delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
			  report.Report.Append("PROTECTEDSWITCH ID = ").Append(s.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
		    }
		    else
		    {
			  report.Report.Append("PROTECTEDSWITCH ID = ").Append(s.ID).AppendLine(" FAILED to be converted");
		    }
		}
		report.Report.AppendLine();
	  }

	  private ResourceDescription CreateProtectedSwitchResourceDescription(ProtectedSwitch s)
	  {
		ResourceDescription rd = null;
		if (s != null)
		{
		    long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.PROTSWITCH, importHelper.CheckOutIndexForDMSType(DMSType.PROTSWITCH));
		    rd = new ResourceDescription(gid);
		    importHelper.DefineIDMapping(s.ID, gid);
		    PowerTransformerConverter.PopulateProtectedSwitchProperties(s, rd, importHelper, report);
		}
		return rd;
	  }
    }
}

