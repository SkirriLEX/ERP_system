//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace erp
{
    using System;
    using System.Collections.Generic;
    
    public partial class Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Group()
        {
            this.Students = new HashSet<Student>();
        }
    
        public Nullable<System.Guid> specializationCode { get; set; }
        public int codeGroup { get; set; }
        public int learningForm { get; set; }
        public string nameGroup { get; set; }
        public int studentLeadId { get; set; }
        public Nullable<System.Guid> teacherLeadId { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Specialization Specialization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Student> Students { get; set; }
    }
}