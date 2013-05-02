using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.Entities
{
    public class BaseContent
    {
        public int Id { get; set; }
        public bool IsNew()
        {
            return Id == 0;
        }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        private DateTime _dateCreated = DateTime.MinValue;
        public DateTime DateCreated
        {
            get
            {
                if (_dateCreated == DateTime.MinValue)
                    DateCreated = DateTime.Now;
                return _dateCreated;
            }
            set
            {
                _dateCreated = value;
            }
        }
        public DateTime DateModified { get; set; }
        public int SortOrder { get; set; }
        private int _priority = 1;
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        private bool _showTitle = true;
        public bool ShowTitle
        {
            get { return _showTitle; }
            set { _showTitle = value; }
        }
    }
}