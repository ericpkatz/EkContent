﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EKContent.web.Models.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public bool PostRequest { get; set; }

        public bool GetRequest
        {
            get { return !PostRequest;  }
        }
    }
}