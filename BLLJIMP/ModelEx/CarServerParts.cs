using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZentCloud.BLLJIMP.Model
{
    public class CarServerParts:Model.CommRelationInfo
    {
        public Model.CarPartsInfo Parts { get; set; }

        public static CarServerParts GetObjByCommRelation(Model.CommRelationInfo r)
        {
            CarServerParts result = new CarServerParts();

            result.AutoId = r.AutoId;
            result.ExpandId = r.ExpandId;
            result.MainId = r.MainId;
            result.RelationId = r.RelationId;
            result.RelationTime = r.RelationTime;
            result.RelationType = r.RelationType;
            result.Ex1 = r.Ex1;
            result.Ex2 = r.Ex2;

            if (!string.IsNullOrWhiteSpace(r.RelationId))
            {
                result.Parts = new BLLCarLibrary().GetPartById(Convert.ToInt32(r.RelationId));
            }
            
            return result;
        }
    }
}
