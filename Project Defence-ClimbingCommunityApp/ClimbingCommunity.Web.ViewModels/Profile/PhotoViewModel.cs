﻿namespace ClimbingCommunity.Web.ViewModels.Profile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PhotoViewModel
    {
        public int Id { get; set; }
        
        public string ImageUrl { get; set; } = null!;
    }
}