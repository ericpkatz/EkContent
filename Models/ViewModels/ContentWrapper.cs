using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EKContent.web.Models.Entities;

namespace EKContent.web.Models.ViewModels
{
    public class ContentWrapper
    {
        public Content Content { get; set; }
        public Page Page { get; set; }
        public int Idx { get; set; }
        public bool EditMode { get; set; }
        public int Mdx { get; set; }
    }
}