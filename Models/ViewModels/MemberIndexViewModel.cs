using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.ViewModels
{
    public class MemberIndexViewModel : BaseViewModel
    {
        public List<Entities.EKRole> Roles { get; set; }
    }
}