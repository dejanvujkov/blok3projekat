using FTN.Services.NetworkModelService.DataModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Outage
{
    public class OutageSchedule : IrregularIntervalSchedule
    {
        public OutageSchedule(long globalId) : base(globalId)
        {
        }

        private List<long> psrs = new List<long>();

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object x)
        {
            OutageSchedule o = (OutageSchedule)x;
            if (base.Equals(x))
            {
                return (o.psrs == psrs);
            }
            return false;
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.OUTAGESCHEDULE_PSRS:
                    return true;
                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property property)
        {
            switch(property.Id)
            {
                case ModelCode.OUTAGESCHEDULE_PSRS:
                    property.SetValue(psrs);
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

        public override bool IsReferenced => psrs.Count > 1 || base.IsReferenced;

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (psrs.Count > 1 && (refType == TypeOfReference.Reference || refType == TypeOfReference.Both))
            {
                references[ModelCode.OUTAGESCHEDULE_PSRS] = new List<long>();
                references[ModelCode.OUTAGESCHEDULE_PSRS] = psrs.GetRange(0, psrs.Count);
                return;
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.PSR_OUTAGESCHEDULE:
                    psrs.Add(globalId);
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
                case ModelCode.PSR_OUTAGESCHEDULE:
                    if (!psrs.Remove(globalId))
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }
                    break;
                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }
    }
}
