using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tipstaff.MemoryCollections
{
    public class ChildRelationship
    {
        public int ChildRelationshipID { get; set; }

        public string Detail { get; set; }

        public int Active { get; set; }

        public int Sequence { get; set; }
    }

    public class ChildRelationshipList
    {
        public static List<ChildRelationship> GetChildRelationshipList()
        {
            return new List<ChildRelationship>()
            {
                new ChildRelationship() { ChildRelationshipID=1, Detail="None-Warrant", Active=0},
                new ChildRelationship() { ChildRelationshipID=2, Detail="Father", Active=1},
                new ChildRelationship() { ChildRelationshipID=3, Detail="Mother", Active=1},
                new ChildRelationship() { ChildRelationshipID=4, Detail="Grandfather", Active=1},
                new ChildRelationship() { ChildRelationshipID=5, Detail="Grandmother", Active=1},
                new ChildRelationship() { ChildRelationshipID=6, Detail="Uncle", Active=1},
                new ChildRelationship() { ChildRelationshipID=7, Detail="Aunt", Active=1},
                new ChildRelationship() { ChildRelationshipID=8, Detail="Brother", Active=1},
                new ChildRelationship() { ChildRelationshipID=9, Detail="Sister", Active=1},
                new ChildRelationship() { ChildRelationshipID=10, Detail="Cousin", Active=1},
                new ChildRelationship() { ChildRelationshipID=11, Detail="Boyfriend", Active=1},
                new ChildRelationship() { ChildRelationshipID=12, Detail="Girlfriend", Active=1},
                new ChildRelationship() { ChildRelationshipID=13, Detail="Guardian", Active=1},
                new ChildRelationship() { ChildRelationshipID=14, Detail="Other", Active=1},
                new ChildRelationship() { ChildRelationshipID=15, Detail="Step Mother", Active=1},
                new ChildRelationship() { ChildRelationshipID=16, Detail="Step Father", Active=1},
                new ChildRelationship() { ChildRelationshipID=17, Detail="Step Sister", Active=1},
                new ChildRelationship() { ChildRelationshipID=18, Detail="Step Brother", Active=1},
                new ChildRelationship() { ChildRelationshipID=19, Detail="Foster Carer", Active=1},
                new ChildRelationship() { ChildRelationshipID=20, Detail="Adoptive Mother", Active=1},
                new ChildRelationship() { ChildRelationshipID=21, Detail="Adoptive Father", Active=1},
                new ChildRelationship() { ChildRelationshipID=22, Detail="Not Applicable", Active=1}
            };
        }
    }
}